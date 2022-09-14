using System;
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

        public DragNewLinkBehavior(Diagram diagram) : base(diagram)
        {
            Diagram.PointerDown += OnPointerDown;
            Diagram.PointerMove += OnPointerMove;
            Diagram.PointerUp += OnPointerUp;
        }

        public void StartFrom(Anchor source, double clientX, double clientY)
        {
            if (_ongoingLink != null)
                return;

            //_ongoingLink = Diagram.Options.Links.Factory(Diagram, port);
            StartFrom(new LinkModel(source), clientX, clientY);
        }

        public void StartFrom(BaseLinkModel link, double clientX, double clientY)
        {
            if (_ongoingLink != null)
                return;

            _ongoingLink = link;
            _ongoingLink.OnGoingPosition = Diagram.GetRelativeMousePoint(clientX, clientY).Substract(5);
            Diagram.Links.Add(_ongoingLink);
        }

        private void OnPointerDown(Model? model, MouseEventArgs e)
        {
            if (e.Button != (int)MouseEventButton.Left)
                return;

            if (model is PortModel port)
            {
                if (port.Locked)
                    return;

                _ongoingLink = Diagram.Options.Links.Factory(Diagram, port);
                if (_ongoingLink == null)
                    return;

                _ongoingLink.OnGoingPosition = Diagram.GetRelativeMousePoint(e.ClientX, e.ClientY).Substract(5);
                Diagram.Links.Add(_ongoingLink);
            }
        }

        private void OnPointerMove(Model? model, MouseEventArgs e)
        {
            if (_ongoingLink == null || model != null)
                return;

            _ongoingLink.OnGoingPosition = Diagram.GetRelativeMousePoint(e.ClientX, e.ClientY).Substract(5);

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

        private void OnPointerUp(Model? model, MouseEventArgs e)
        {
            if (_ongoingLink == null)
                return;

            if (_ongoingLink.IsAttached) // Snapped already
            {
                _ongoingLink = null;
                return;
            }

            if (model is ILinkable linkable && _ongoingLink.Source.Model.CanAttachTo(linkable))
            {
                var targetAnchor = Diagram.Options.Links.TargetAnchorFactory(Diagram, _ongoingLink, linkable);
                _ongoingLink.OnGoingPosition = null;
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
            foreach (var port in Diagram.Nodes.SelectMany(n => n.Ports))
            {
                if (_ongoingLink!.OnGoingPosition!.DistanceTo(port.MiddlePosition) < Diagram.Options.Links.SnappingRadius 
                    && _ongoingLink.Source.Model.CanAttachTo(port))
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