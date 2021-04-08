using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models.Base;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Linq;

namespace Blazor.Diagrams.Core.Behaviors
{
    public class DragMovablesBehavior : Behavior
    {
        private Point[]? _initialPositions;
        private double? _lastClientX;
        private double? _lastClientY;

        public DragMovablesBehavior(Diagram diagram) : base(diagram)
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
            => Move(e.ChangedTouches[0].ClientX, e.ChangedTouches[0].ClientY);

        private void OnTouchEnd(Model model, TouchEventArgs e) => End();

        private void OnMouseDown(Model model, MouseEventArgs e) => Start(model, e.ClientX, e.ClientY);

        private void OnMouseMove(Model model, MouseEventArgs e) => Move(e.ClientX, e.ClientY);

        private void OnMouseUp(Model model, MouseEventArgs e) => End();

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
                if (!(sm is MovableModel node) || node.Locked)
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
            return gridSize * Math.Floor((n + gridSize / 2) / gridSize);
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
