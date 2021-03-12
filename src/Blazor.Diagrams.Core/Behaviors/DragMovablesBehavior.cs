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
            Diagram.MouseDown += Diagram_MouseDown;
            Diagram.MouseMove += Diagram_MouseMove;
            Diagram.MouseUp += Diagram_MouseUp;
        }

        private void Diagram_MouseDown(Model model, MouseEventArgs e)
        {
            if (!(model is MovableModel))
                return;

            // Don't like this linq
            _initialPositions = Diagram.GetSelectedModels()
                .Where(m => m is MovableModel)
                .Select(m => (m as MovableModel)!.Position)
                .ToArray();

            _lastClientX = e.ClientX;
            _lastClientY = e.ClientY;
        }

        private void Diagram_MouseMove(Model model, MouseEventArgs e)
        {
            if (_initialPositions == null || _lastClientX == null || _lastClientY == null)
                return;

            var deltaX = (e.ClientX - _lastClientX.Value) / Diagram.Zoom;
            var deltaY = (e.ClientY - _lastClientY.Value) / Diagram.Zoom;
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

        private void Diagram_MouseUp(Model model, MouseEventArgs e)
        {
            _initialPositions = null;
            _lastClientX = null;
            _lastClientY = null;
        }

        public override void Dispose()
        {
            Diagram.MouseDown -= Diagram_MouseDown;
            Diagram.MouseMove -= Diagram_MouseMove;
            Diagram.MouseUp -= Diagram_MouseUp;
        }
    }
}
