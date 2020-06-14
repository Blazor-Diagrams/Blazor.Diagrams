using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;
using Microsoft.AspNetCore.Components.Web;

namespace Blazor.Diagrams.Core.Default
{
    public class DragNewLinkSubManager : DiagramSubManager
    {
        private double _initialX;
        private double _initialY;
        private LinkModel? _ongoingLink;

        public DragNewLinkSubManager(DiagramManager diagramManager) : base(diagramManager)
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
            _ongoingLink = DiagramManager.AddLink(port, null);
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
                DiagramManager.RemoveLink(_ongoingLink);
                _ongoingLink = null;
                return;
            }

            DiagramManager.AttachLink(_ongoingLink, port);
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
