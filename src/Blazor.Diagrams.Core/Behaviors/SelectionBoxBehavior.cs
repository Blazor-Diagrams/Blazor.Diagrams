using Blazor.Diagrams.Core.Behaviors.Base;
using Blazor.Diagrams.Core.Events;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models.Base;
using System;
using System.Linq;

namespace Blazor.Diagrams.Core.Behaviors
{
    public class SelectionBoxBehavior : DragBehavior
    {
        private Point? _initialClientPoint;

        public event EventHandler<Rectangle?>? SelectionBoundsChanged;

        public SelectionBoxBehavior(Diagram diagram)
            : base(diagram)
        {
            Diagram.PointerDown += OnPointerDown;
            Diagram.PointerMove += OnPointerMove;
            Diagram.PointerUp += OnPointerUp;
        }

        public override void Dispose()
        {
            Diagram.PointerDown -= OnPointerDown;
            Diagram.PointerMove -= OnPointerMove;
            Diagram.PointerUp -= OnPointerUp;
        }

        protected override void OnPointerDown(Model? model, PointerEventArgs e)
        {
            if (SelectionBoundsChanged is null || model != null || !IsBehaviorEnabled(e))
                return;

            _initialClientPoint = new Point(e.ClientX, e.ClientY);
        }

        protected override void OnPointerMove(Model? model, PointerEventArgs e)
        {
            if (_initialClientPoint == null)
                return;

            UpdateSelectionBox(e);

            var start = Diagram.GetRelativeMousePoint(_initialClientPoint.X, _initialClientPoint.Y);
            var end = Diagram.GetRelativeMousePoint(e.ClientX, e.ClientY);
            var (sX, sY) = (Math.Min(start.X, end.X), Math.Min(start.Y, end.Y));
            var (eX, eY) = (Math.Max(start.X, end.X), Math.Max(start.Y, end.Y));
            var bounds = new Rectangle(sX, sY, eX, eY);

            foreach (var node in Diagram.Nodes)
            {
                var nodeBounds = node.GetBounds();
                if (nodeBounds == null)
                    continue;

                if (bounds.Overlap(nodeBounds))
                    Diagram.SelectModel(node, false);
                else if (node.Selected) Diagram.UnselectModel(node);
            }
        }

        void UpdateSelectionBox(MouseEventArgs e)
        {
            var start = Diagram.GetRelativePoint(_initialClientPoint!.X, _initialClientPoint.Y);
            var end = Diagram.GetRelativePoint(e.ClientX, e.ClientY);
            var (sX, sY) = (Math.Min(start.X, end.X), Math.Min(start.Y, end.Y));
            var (eX, eY) = (Math.Max(start.X, end.X), Math.Max(start.Y, end.Y));
            SelectionBoundsChanged?.Invoke(this, new Rectangle(sX, sY, eX, eY));
        }

        protected override void OnPointerUp(Model? model, PointerEventArgs e)
        {
            _initialClientPoint = null;
            SelectionBoundsChanged?.Invoke(this, null);
        }
    }
}
