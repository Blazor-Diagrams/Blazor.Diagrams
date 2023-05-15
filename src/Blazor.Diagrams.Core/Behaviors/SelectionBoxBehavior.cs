using Blazor.Diagrams.Core.Events;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models.Base;
using Blazor.Diagrams.Core.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazor.Diagrams.Core.Behaviors
{
    public class SelectionBoxBehavior : Behavior
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

        private void OnPointerDown(Model? model, PointerEventArgs e)
        {
            if (SelectionBoundsChanged is null || model != null || !Diagram.IsBehaviorEnabled(e, DiagramDragBehavior.Select))
                return;

            _initialClientPoint = new Point(e.ClientX, e.ClientY);
        }

        private void OnPointerMove(Model? model, PointerEventArgs e)
        {
            if (_initialClientPoint == null)
                return;

            var start = Diagram.GetRelativeMousePoint(_initialClientPoint.X, _initialClientPoint.Y);
            var end = Diagram.GetRelativeMousePoint(e.ClientX, e.ClientY);
            var (sX, sY) = (Math.Min(start.X, end.X), Math.Min(start.Y, end.Y));
            var (eX, eY) = (Math.Max(start.X, end.X), Math.Max(start.Y, end.Y));
            var bounds = new Rectangle(sX, sY, eX, eY);

            SelectionBoundsChanged?.Invoke(this, bounds);

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

        private void OnPointerUp(Model? model, PointerEventArgs e)
        {
            _initialClientPoint = null;
            SelectionBoundsChanged?.Invoke(this, null);
        }
    }
}
