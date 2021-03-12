using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;
using Microsoft.AspNetCore.Components.Web;
using System.Linq;

namespace Blazor.Diagrams.Core.Behaviors
{
    public class DragNewLinkBehavior : Behavior
    {
        private double _initialX;
        private double _initialY;
        private BaseLinkModel? _ongoingLink;

        public DragNewLinkBehavior(Diagram diagram) : base(diagram)
        {
            Diagram.MouseDown += Diagram_MouseDown;
            Diagram.MouseMove += Diagram_MouseMove;
            Diagram.MouseUp += Diagram_MouseUp;
        }

        private void Diagram_MouseDown(Model model, MouseEventArgs e)
        {
            if (!(model is PortModel port) || port.Locked || e.Button != (int)MouseEventButton.Left)
                return;

            _initialX = e.ClientX;
            _initialY = e.ClientY;
            _ongoingLink = Diagram.Options.Links.Factory(Diagram, port);
            _ongoingLink.OnGoingPosition = new Point(port.Position.X + port.Size.Width / 2,
                port.Position.Y + port.Size.Height / 2);
            Diagram.Links.Add(_ongoingLink);
        }

        private void Diagram_MouseMove(Model model, MouseEventArgs e)
        {
            if (_ongoingLink == null || model != null)
                return;

            var deltaX = (e.ClientX - _initialX) / Diagram.Zoom;
            var deltaY = (e.ClientY - _initialY) / Diagram.Zoom;
            var sX = _ongoingLink.SourcePort!.Position.X + _ongoingLink.SourcePort.Size.Width / 2;
            var sY = _ongoingLink.SourcePort.Position.Y + _ongoingLink.SourcePort.Size.Height / 2;

            _ongoingLink.OnGoingPosition = new Point(sX + deltaX, sY + deltaY);

            if (Diagram.Options.Links.EnableSnapping)
            {
                var nearPort = FindNearPortToAttachTo();
                if (nearPort != null || _ongoingLink.TargetPort != null)
                {
                    var oldPort = _ongoingLink.TargetPort;
                    _ongoingLink.SetTargetPort(nearPort);
                    oldPort?.Refresh();
                    nearPort?.Refresh();
                }
            }

            _ongoingLink.Refresh();
        }

        private void Diagram_MouseUp(Model model, MouseEventArgs e)
        {
            if (_ongoingLink == null)
                return;

            if (_ongoingLink.IsAttached) // Snapped already
            {
                _ongoingLink = null;
                return;
            }

            if (!(model is PortModel port) || !_ongoingLink.SourcePort!.CanAttachTo(port))
            {
                Diagram.Links.Remove(_ongoingLink);
                _ongoingLink = null;
                return;
            }

            _ongoingLink.OnGoingPosition = null;
            _ongoingLink.SetTargetPort(port);
            _ongoingLink.Refresh();
            port.Refresh();
            _ongoingLink.SourcePort.Parent.Group?.Refresh();
            port?.Parent.Group?.Refresh();
            _ongoingLink = null;
        }

        private PortModel? FindNearPortToAttachTo()
        {
            foreach (var port in Diagram.Nodes.SelectMany(n => n.Ports))
            {
                if (_ongoingLink!.OnGoingPosition!.DistanceTo(port.Position) < Diagram.Options.Links.SnappingRadius &&
                    _ongoingLink.SourcePort!.CanAttachTo(port))
                    return port;
            }

            return null;
        }

        public override void Dispose()
        {
            Diagram.MouseDown -= Diagram_MouseDown;
            Diagram.MouseMove -= Diagram_MouseMove;
            Diagram.MouseUp -= Diagram_MouseUp;
        }
    }
}
