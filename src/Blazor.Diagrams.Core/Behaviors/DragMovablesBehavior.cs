using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models.Base;
using Blazor.Diagrams.Core.Events;
using System;
using System.Collections.Generic;
using Blazor.Diagrams.Core.Models;

namespace Blazor.Diagrams.Core.Behaviors
{
    public class DragMovablesBehavior : Behavior
    {
        private readonly Dictionary<NodeModel, Point> _initialPositions;
        private double? _lastClientX;
        private double? _lastClientY;
        private bool _moved;

        public DragMovablesBehavior(Diagram diagram) : base(diagram)
        {
            _initialPositions = new Dictionary<NodeModel, Point>();
            Diagram.PointerDown += OnPointerDown;
            Diagram.PointerMove += OnPointerMove;
            Diagram.PointerUp += OnPointerUp;
        }

        private void OnPointerDown(Model? model, PointerEventArgs e)
        {
            if (model is not NodeModel)
                return;

            _initialPositions.Clear();
            foreach (var sm in Diagram.GetSelectedModels())
            {
                if (sm is not NodeModel movable || movable.Locked)
                    continue;
                var position = movable.Position;
                if (Diagram.Options.GridSnapToCenter)
                {
                    position = new Point(movable.Position.X + movable.Size.Width / 2,
                        movable.Position.Y + movable.Size.Height / 2);
                }
                _initialPositions.Add(movable, position);
            }

            _lastClientX = e.ClientX;
            _lastClientY = e.ClientY;
            _moved = false;
        }

        private void OnPointerMove(Model? model, PointerEventArgs e)
        {
            if (_initialPositions.Count == 0 || _lastClientX == null || _lastClientY == null)
                return;

            _moved = true;
            var deltaX = (e.ClientX - _lastClientX.Value) / Diagram.Zoom;
            var deltaY = (e.ClientY - _lastClientY.Value) / Diagram.Zoom;

            foreach (var (node, initialPosition) in _initialPositions)
            {
                var ndx = ApplyGridSize(deltaX + initialPosition.X);
                var ndy = ApplyGridSize(deltaY + initialPosition.Y);
                if (Diagram.Options.GridSnapToCenter)
                    node.SetPosition(ndx - node.Size.Width / 2, ndy - node.Size.Height / 2);
                else
                    node.SetPosition(ndx, ndy);
            }
        }

        private void OnPointerUp(Model? model, PointerEventArgs e)
        {
            if (_initialPositions.Count == 0)
                return;

            if (_moved)
            {
                foreach (var (movable, _) in _initialPositions)
                {
                    movable.TriggerMoved();
                }
            }
            
            _initialPositions.Clear();
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
            _initialPositions.Clear();
            
            Diagram.PointerDown -= OnPointerDown;
            Diagram.PointerMove -= OnPointerMove;
            Diagram.PointerUp -= OnPointerUp;
        }
    }
}
