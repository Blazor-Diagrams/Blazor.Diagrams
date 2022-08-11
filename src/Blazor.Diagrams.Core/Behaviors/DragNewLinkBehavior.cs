using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;
using Blazor.Diagrams.Core.Events;
using System.Linq;
using Blazor.Diagrams.Core.Anchors;

namespace Blazor.Diagrams.Core.Behaviors
{
    public class DragNewLinkBehavior : Behavior
    {
        private double _initialX;
        private double _initialY;
        private BaseLinkModel? _ongoingLink;

        public DragNewLinkBehavior(DiagramBase diagram) : base(diagram)
        {
            Diagram.MouseDown += OnMouseDown;
            Diagram.MouseMove += OnMouseMove;
            Diagram.MouseUp += OnMouseUp;
            Diagram.TouchStart += OnTouchStart;
            Diagram.TouchMove += OnTouchMove;
            Diagram.TouchEnd += OnTouchEnd;
        }

        private void OnTouchStart(Model? model, TouchEventArgs e)
            => Start(model, e.ChangedTouches[0].ClientX, e.ChangedTouches[0].ClientY);

        private void OnTouchMove(Model? model, TouchEventArgs e)
            => Move(model, e.ChangedTouches[0].ClientX, e.ChangedTouches[0].ClientY);

        private void OnTouchEnd(Model? model, TouchEventArgs e) => End(model);

        private void OnMouseDown(Model? model, MouseEventArgs e)
        {
            if (e.Button != (int)MouseEventButton.Left)
                return;

            Start(model, e.ClientX, e.ClientY);
        }

        private void OnMouseMove(Model? model, MouseEventArgs e) => Move(model, e.ClientX, e.ClientY);

        private void OnMouseUp(Model? model, MouseEventArgs e) => End(model);

        private void Start(Model? model, double clientX, double clientY)
        {
            if (model is PortModel port)
            {
                if (port.Locked) return;
                _ongoingLink = Diagram.Options.Links.Factory(Diagram, port);
                _ongoingLink.OnGoingPosition = Diagram.GetRelativeMousePoint(clientX, clientY).Substract(5);
            }
            else
            {
                return;
            }

            _initialX = clientX;
            _initialY = clientY;
            Diagram.Links.Add(_ongoingLink);
        }

        private void Move(Model? model, double clientX, double clientY)
        {
            if (_ongoingLink == null || model != null)
                return;

            _ongoingLink.OnGoingPosition = Diagram.GetRelativeMousePoint(clientX, clientY).Substract(5);

            if (Diagram.Options.Links.EnableSnapping)
            {
                var nearPort = FindNearPortToAttachTo();
                if (nearPort != null || _ongoingLink.Target != null)
                {
                    _ongoingLink.SetTarget(nearPort is null ? null : new SinglePortAnchor(nearPort));
                }
            }

            _ongoingLink.Refresh();
        }

        private void End(Model? model)
        {
            if (_ongoingLink == null)
                return;

            if (_ongoingLink.IsAttached) // Snapped already
            {
                _ongoingLink = null;
                return;
            }

            var sourcePort = (_ongoingLink.Source as SinglePortAnchor)!.Port; // Assumption for now

            if (model is not PortModel port || !sourcePort.CanAttachTo(port))
            {
                Diagram.Links.Remove(_ongoingLink);
                _ongoingLink = null;
                return;
            }

            _ongoingLink.OnGoingPosition = null;
            _ongoingLink.SetTarget(new SinglePortAnchor(port));
            _ongoingLink.Refresh();
            sourcePort.Parent.Group?.Refresh();
            port?.Parent.Group?.Refresh();
            _ongoingLink = null;
        }

        private PortModel? FindNearPortToAttachTo()
        {
            var sourcePort = (_ongoingLink!.Source as SinglePortAnchor)!.Port; // Assumption for now

            foreach (var port in Diagram.Nodes.SelectMany(n => n.Ports))
            {
                if (_ongoingLink!.OnGoingPosition!.DistanceTo(port.MiddlePosition) < Diagram.Options.Links.SnappingRadius && sourcePort.CanAttachTo(port))
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
