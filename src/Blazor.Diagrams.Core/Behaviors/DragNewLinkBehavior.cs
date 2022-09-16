using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;
using Blazor.Diagrams.Core.Events;
using System.Linq;
using Blazor.Diagrams.Core.Anchors;

namespace Blazor.Diagrams.Core.Behaviors
{
    public class DragNewLinkBehavior : Behavior
    {
        private BaseLinkModel? _ongoingLink;
        private PositionAnchor? _targetPositionAnchor;

        public DragNewLinkBehavior(Diagram diagram) : base(diagram)
        {
            Diagram.PointerDown += OnPointerDown;
            Diagram.PointerMove += OnPointerMove;
            Diagram.PointerUp += OnPointerUp;
        }

        public void StartFrom(ILinkable source, double clientX, double clientY)
        {
            if (_ongoingLink != null)
                return;

            _targetPositionAnchor = new PositionAnchor(Diagram.GetRelativeMousePoint(clientX, clientY).Substract(5));
            _ongoingLink = Diagram.Options.Links.Factory(Diagram, source, _targetPositionAnchor);
            if (_ongoingLink == null)
                return;

            Diagram.Links.Add(_ongoingLink);
        }

        private void OnPointerDown(Model? model, MouseEventArgs e)
        {
            if (e.Button != (int)MouseEventButton.Left)
                return;

            _ongoingLink = null;
            _targetPositionAnchor = null;

            if (model is PortModel port)
            {
                if (port.Locked)
                    return;

                _targetPositionAnchor = new PositionAnchor(Diagram.GetRelativeMousePoint(e.ClientX, e.ClientY).Substract(5));
                _ongoingLink = Diagram.Options.Links.Factory(Diagram, port, _targetPositionAnchor);
                if (_ongoingLink == null)
                    return;

                _ongoingLink.SetTarget(_targetPositionAnchor);
                Diagram.Links.Add(_ongoingLink);
            }
        }

        private void OnPointerMove(Model? model, MouseEventArgs e)
        {
            if (_ongoingLink == null || model != null)
                return;

            _targetPositionAnchor!.SetPosition(Diagram.GetRelativeMousePoint(e.ClientX, e.ClientY).Substract(5));

            if (Diagram.Options.Links.EnableSnapping)
            {
                var nearPort = FindNearPortToAttachTo();
                if (nearPort != null || _ongoingLink.Target is not PositionAnchor)
                {
                    _ongoingLink.SetTarget(nearPort is null ? _targetPositionAnchor : new SinglePortAnchor(nearPort));
                }
            }

            _ongoingLink.Refresh();
        }

        private void OnPointerUp(Model? model, MouseEventArgs e)
        {
            if (_ongoingLink == null)
                return;

            if (_ongoingLink.IsAttached) // Snapped already
            {
                _ongoingLink = null;
                return;
            }

            if (model is ILinkable linkable && (_ongoingLink.Source.Model == null || _ongoingLink.Source.Model.CanAttachTo(linkable)))
            {
                var targetAnchor = Diagram.Options.Links.TargetAnchorFactory(Diagram, _ongoingLink, linkable);
                _ongoingLink.SetTarget(targetAnchor);
                _ongoingLink.Refresh();
            }
            else if (Diagram.Options.Links.RequireTarget)
            {
                Diagram.Links.Remove(_ongoingLink);
            }

            _ongoingLink = null;
        }

        private PortModel? FindNearPortToAttachTo()
        {
            var ongoingPosition = _targetPositionAnchor!.GetPosition(_ongoingLink!)!;
            foreach (var port in Diagram.Nodes.SelectMany(n => n.Ports))
            {
                if (ongoingPosition.DistanceTo(port.MiddlePosition) < Diagram.Options.Links.SnappingRadius
                    && (_ongoingLink!.Source.Model == null || _ongoingLink.Source.Model.CanAttachTo(port)))
                {
                    return port;
                }
            }

            return null;
        }

        public override void Dispose()
        {
            Diagram.PointerDown -= OnPointerDown;
            Diagram.PointerMove -= OnPointerMove;
            Diagram.PointerUp -= OnPointerUp;
        }
    }
}