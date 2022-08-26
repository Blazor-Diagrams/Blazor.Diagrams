using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models.Base;
using Blazor.Diagrams.Core.Events;
using System;
using System.Linq;

namespace Blazor.Diagrams.Core.Behaviors
{
    public class DragMovablesBehavior : Behavior
    {
        private Point[]? _initialPositions;
        private double? _lastClientX;
        private double? _lastClientY;

        public DragMovablesBehavior(DiagramBase diagram) : base(diagram)
        {
            Diagram.PointerDown += OnPointerDown;
            Diagram.PointerMove += OnPointerMove;
            Diagram.PointerUp += OnPointerUp;
        }

        private void OnPointerDown(Model? model, PointerEventArgs e) => Start(model, e.ClientX, e.ClientY);

        private void OnPointerMove(Model? model, PointerEventArgs e) => Move(e.ClientX, e.ClientY);

        private void OnPointerUp(Model? model, PointerEventArgs e) => End();

        private void Start(Model model, double clientX, double clientY)
        {
            if (!(model is MovableModel))
                return;

            // Don't like this linq
            _initialPositions = Diagram.GetSelectedModels()
                .Where(m => m is MovableModel)
                .Select(m => (m as MovableModel)!.Position)
                .ToArray();

            _lastClientX = clientX;
            _lastClientY = clientY;
        }

        private void Move(double clientX, double clientY)
        {
            if (_initialPositions == null || _lastClientX == null || _lastClientY == null)
                return;

            var deltaX = (clientX - _lastClientX.Value) / Diagram.Zoom;
            var deltaY = (clientY - _lastClientY.Value) / Diagram.Zoom;
            var i = 0;

            foreach (var sm in Diagram.GetSelectedModels())
            {
                if (sm is not MovableModel node || node.Locked)
                    continue;

                var initialPosition = _initialPositions[i];
                var ndx = ApplyGridSize(deltaX + initialPosition.X);
                var ndy = ApplyGridSize(deltaY + initialPosition.Y);
                node.SetPosition(ndx, ndy);
                i++;
            }
        }

        private void End()
        {
            _initialPositions = null;
            _lastClientX = null;
            _lastClientY = null;
        }

        private double ApplyGridSize(double n)
        {
            if (Diagram.Options.GridSize == null)
                return n;

            var gridSize = Diagram.Options.GridSize.Value;

            // 20 * floor((100 + 10) / 20) = 20 * 5 = 100
            // 20 * floor((105 + 10) / 20) = 20 * 5 = 100
            // 20 * floor((110 + 10) / 20) = 20 * 6 = 120
            return gridSize * Math.Floor((n + gridSize / 2.0) / gridSize);
        }

        public override void Dispose()
        {
            Diagram.PointerDown -= OnPointerDown;
            Diagram.PointerMove -= OnPointerMove;
            Diagram.PointerUp -= OnPointerUp;
        }
    }
}
