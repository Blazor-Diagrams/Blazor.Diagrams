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
        private double? _lastClientX;
        private double? _lastClientY;
        private Point? _initialPan;

        public SelectionBoxBehavior(Diagram diagram)
            : base(diagram)
        {
            Diagram.PointerDown += OnPointerDown;
            Diagram.PointerMove += OnPointerMove;
            Diagram.PointerUp += OnPointerUp;
            Diagram.PanChanged += OnPanChanged;
        }

        public override void Dispose()
        {
            Diagram.PointerDown -= OnPointerDown;
            Diagram.PointerMove -= OnPointerMove;
            Diagram.PointerUp -= OnPointerUp;
            Diagram.PanChanged -= OnPanChanged;
        }

        protected override void OnPointerDown(Model? model, PointerEventArgs e)
        {
            if (SelectionBoundsChanged is null || model != null || !IsBehaviorEnabled(e))
                return;

            _initialClientPoint = new Point(e.ClientX, e.ClientY);
            _lastClientX = e.ClientX;
            _lastClientY = e.ClientY;
            _initialPan = Diagram.Pan;
        }

        protected override void OnPointerMove(Model? model, PointerEventArgs e)
        {
            if (_initialClientPoint == null)
                return;

            _lastClientX = e.ClientX;
            _lastClientY = e.ClientY;

            UpdateSelectionBox(e.ClientX, e.ClientY);
            SelectNodesInBounds(e.ClientX, e.ClientY);
        }

        void UpdateSelectionBox(double clientX, double clientY)
        {
            if(_initialClientPoint == null || _initialPan == null)
            {
                return;
            }

            var start = Diagram.GetRelativePoint(_initialClientPoint.X + Diagram.Pan.X - _initialPan.X, _initialClientPoint.Y + Diagram.Pan.Y - _initialPan.Y);
            var end = Diagram.GetRelativePoint(clientX, clientY);
            var (sX, sY) = (Math.Min(start.X, end.X), Math.Min(start.Y, end.Y));
            var (eX, eY) = (Math.Max(start.X, end.X), Math.Max(start.Y, end.Y));
            SelectionBoundsChanged?.Invoke(this, new Rectangle(sX, sY, eX, eY));
        }

        protected override void OnPointerUp(Model? model, PointerEventArgs e)
        {
            _initialClientPoint = null;
            SelectionBoundsChanged?.Invoke(this, null);
            _lastClientX = null;
            _lastClientY = null;
            _initialPan = null;
        }

        public void OnPanChanged(double deltaX, double deltaY)
        {
            if (_initialClientPoint == null || _lastClientX == null || _lastClientY == null)
                return;

            UpdateSelectionBox((double) _lastClientX, (double) _lastClientY);
            SelectNodesInBounds((double) _lastClientX, (double) _lastClientY);
        }

        void SelectNodesInBounds(double clientX, double clientY)
        {
            if(_initialClientPoint == null || _initialPan == null)
            {
                return;
            }

            var start = Diagram.GetRelativeMousePoint(_initialClientPoint.X + Diagram.Pan.X - _initialPan.X, _initialClientPoint.Y + Diagram.Pan.Y - _initialPan.Y);
            var end = Diagram.GetRelativeMousePoint(clientX, clientY);
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
    }
}
