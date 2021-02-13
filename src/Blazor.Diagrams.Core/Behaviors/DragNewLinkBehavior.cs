using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;
using Blazor.Diagrams.Core.Models.Core;
using Microsoft.AspNetCore.Components.Web;

namespace Blazor.Diagrams.Core.Behaviors
{
    public class DragNewLinkBehavior : Behavior
    {
        private double _initialX;
        private double _initialY;
        private BaseLinkModel? _ongoingLink;

        public DragNewLinkBehavior(DiagramManager diagramManager) : base(diagramManager)
        {
            DiagramManager.MouseDown += DiagramManager_MouseDown;
            DiagramManager.MouseMove += DiagramManager_MouseMove;
            DiagramManager.MouseUp += DiagramManager_MouseUp;
        }

        private void DiagramManager_MouseDown(Model model, MouseEventArgs e)
        {
            if (!(model is PortModel port) || port.Locked)
                return;

            _initialX = e.ClientX;
            _initialY = e.ClientY;
            // Todo: Link creator from Options
            _ongoingLink = new LinkModel(port, null);
            _ongoingLink.OnGoingPosition = new Point(port.Position.X + port.Size.Width / 2,
                port.Position.Y + port.Size.Height / 2);
            DiagramManager.Links.Add(_ongoingLink);
        }

        private void DiagramManager_MouseMove(Model model, MouseEventArgs e)
        {
            if (_ongoingLink == null || model != null)
                return;

            var deltaX = (e.ClientX - _initialX) / DiagramManager.Zoom;
            var deltaY = (e.ClientY - _initialY) / DiagramManager.Zoom;
            var sX = _ongoingLink.SourcePort.Position.X + _ongoingLink.SourcePort.Size.Width / 2;
            var sY = _ongoingLink.SourcePort.Position.Y + _ongoingLink.SourcePort.Size.Height / 2;
            _ongoingLink.OnGoingPosition = new Point(sX + deltaX, sY + deltaY);
            _ongoingLink.Refresh();
        }

        private void DiagramManager_MouseUp(Model model, MouseEventArgs e)
        {
            if (_ongoingLink == null)
                return;

            if (!(model is PortModel port) || !_ongoingLink.SourcePort.CanAttachTo(port))
            {
                DiagramManager.Links.Remove(_ongoingLink);
                _ongoingLink = null;
                return;
            }

            _ongoingLink.SetTargetPort(port);
            _ongoingLink.Refresh();
            port.Refresh();
            _ongoingLink.SourcePort.Parent.Group?.Refresh();
            port?.Parent.Group?.Refresh();
            _ongoingLink = null;
        }

        public override void Dispose()
        {
            DiagramManager.MouseDown -= DiagramManager_MouseDown;
            DiagramManager.MouseMove -= DiagramManager_MouseMove;
            DiagramManager.MouseUp -= DiagramManager_MouseUp;
        }
    }
}
