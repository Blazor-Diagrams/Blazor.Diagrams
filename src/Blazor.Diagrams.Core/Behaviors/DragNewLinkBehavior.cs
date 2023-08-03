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

        public void StartFrom(BaseLinkModel link, double clientX, double clientY)
        {
            if (_ongoingLink != null)
                return;

            _targetPositionAnchor = new PositionAnchor(Diagram.GetRelativeMousePoint(clientX, clientY).Substract(5));
            _ongoingLink = link;
            _ongoingLink.SetTarget(_targetPositionAnchor);
            _ongoingLink.Refresh();
            _ongoingLink.RefreshLinks();
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
            _ongoingLink.RefreshLinks();
        }

        private void OnPointerUp(Model? model, MouseEventArgs e)
        {
            if (_ongoingLink == null)
                return;

            if (_ongoingLink.IsAttached) // Snapped already
            {
                _ongoingLink.TriggerTargetAttached();
                _ongoingLink = null;
                return;
            }

            if (model is ILinkable linkable && (_ongoingLink.Source.Model == null || _ongoingLink.Source.Model.CanAttachTo(linkable)))
            {
                var targetAnchor = Diagram.Options.Links.TargetAnchorFactory(Diagram, _ongoingLink, linkable);
                _ongoingLink.SetTarget(targetAnchor);
                _ongoingLink.TriggerTargetAttached();
                _ongoingLink.Refresh();
                _ongoingLink.RefreshLinks();
            }
            else if (Diagram.Options.Links.RequireTarget)
            {
                Diagram.Links.Remove(_ongoingLink);
            }

            _ongoingLink = null;
        }

        private PortModel? FindNearPortToAttachTo()
        {
            if (_ongoingLink is null || _targetPositionAnchor is null)
                return null;

            PortModel? nearestSnapPort = null;
            var nearestSnapPortDistance = double.PositiveInfinity;

            var position = _targetPositionAnchor!.GetPosition(_ongoingLink)!;

            foreach (var port in Diagram.Nodes.SelectMany((NodeModel n) => n.Ports))
            {
                var distance = position.DistanceTo(port.Position);

                if (distance <= Diagram.Options.Links.SnappingRadius && (_ongoingLink.Source.Model?.CanAttachTo(port) != false))
                {
                    if (distance < nearestSnapPortDistance)
                    {
                        nearestSnapPortDistance = distance;
                        nearestSnapPort = port;
                    }
                }
            }

            return nearestSnapPort;
        }

        public override void Dispose()
        {
            Diagram.PointerDown -= OnPointerDown;
            Diagram.PointerMove -= OnPointerMove;
            Diagram.PointerUp -= OnPointerUp;
        }
    }
}