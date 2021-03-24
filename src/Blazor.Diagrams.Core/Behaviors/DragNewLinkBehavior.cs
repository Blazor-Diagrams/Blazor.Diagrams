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
            Diagram.MouseDown += OnMouseDown;
            Diagram.MouseMove += OnMouseMove;
            Diagram.MouseUp += OnMouseUp;
            Diagram.TouchStart += OnTouchStart;
            Diagram.TouchMove += OnTouchMove;
            Diagram.TouchEnd += OnTouchEnd;
        }

        private void OnTouchStart(Model model, TouchEventArgs e)
            => Start(model, e.ChangedTouches[0].ClientX, e.ChangedTouches[0].ClientY);

        private void OnTouchMove(Model model, TouchEventArgs e)
            => Move(model, e.ChangedTouches[0].ClientX, e.ChangedTouches[0].ClientY);

        private void OnTouchEnd(Model model, TouchEventArgs e) => End(model);

        private void OnMouseDown(Model model, MouseEventArgs e)
        {
            if (e.Button != (int)MouseEventButton.Left)
                return;

            Start(model, e.ClientX, e.ClientY);
        }

        private void OnMouseMove(Model model, MouseEventArgs e) => Move(model, e.ClientX, e.ClientY);

        private void OnMouseUp(Model model, MouseEventArgs e) => End(model);

        private void Start(Model model, double clientX, double clientY)
        {
            if (!(model is PortModel port) || port.Locked)
                return;

            _initialX = clientX;
            _initialY = clientY;
            _ongoingLink = Diagram.Options.Links.Factory(Diagram, port);
            _ongoingLink.OnGoingPosition = new Point(port.Position.X + port.Size.Width / 2,
                port.Position.Y + port.Size.Height / 2);
            Diagram.Links.Add(_ongoingLink);
        }

        private void Move(Model model, double clientX, double clientY)
        {
            if (_ongoingLink == null || model != null)
                return;

            var deltaX = (clientX - _initialX) / Diagram.Zoom;
            var deltaY = (clientY - _initialY) / Diagram.Zoom;
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

        private void End(Model model)
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
            Diagram.MouseDown -= OnMouseDown;
            Diagram.MouseMove -= OnMouseMove;
            Diagram.MouseUp -= OnMouseUp;
            Diagram.TouchStart -= OnTouchStart;
            Diagram.TouchMove -= OnTouchMove;
            Diagram.TouchEnd -= OnTouchEnd;
        }
    }
}
