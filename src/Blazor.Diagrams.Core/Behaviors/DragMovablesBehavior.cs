using Blazor.Diagrams.Core.Models.Base;
using Blazor.Diagrams.Core.Models.Core;
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

        public DragMovablesBehavior(DiagramManager diagramManager) : base(diagramManager)
        {
            DiagramManager.MouseDown += DiagramManager_MouseDown;
            DiagramManager.MouseMove += DiagramManager_MouseMove;
            DiagramManager.MouseUp += DiagramManager_MouseUp;
        }

        private void DiagramManager_MouseDown(Model model, MouseEventArgs e)
        {
            if (!(model is MovableModel))
                return;

            // Don't link this linq
            _initialPositions = DiagramManager.GetSelectedModels()
                .Where(m => m is MovableModel)
                .Select(m => (m as MovableModel)!.Position)
                .ToArray();

            _lastClientX = e.ClientX;
            _lastClientY = e.ClientY;
        }

        private void DiagramManager_MouseMove(Model model, MouseEventArgs e)
        {
            if (_initialPositions == null || _lastClientX == null || _lastClientY == null)
                return;

            var deltaX = (e.ClientX - _lastClientX.Value) / DiagramManager.Zoom;
            var deltaY = (e.ClientY - _lastClientY.Value) / DiagramManager.Zoom;
            var i = 0;

            foreach (var sm in DiagramManager.GetSelectedModels())
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
            if (DiagramManager.Options.GridSize == null)
                return n;

            var gridSize = DiagramManager.Options.GridSize.Value;

            // 20 * floor((100 + 10) / 20) = 20 * 5 = 100
            // 20 * floor((105 + 10) / 20) = 20 * 5 = 100
            // 20 * floor((110 + 10) / 20) = 20 * 6 = 120
            return gridSize * Math.Floor((n + gridSize / 2) / gridSize);
        }

        private void DiagramManager_MouseUp(Model model, MouseEventArgs e)
        {
            _initialPositions = null;
            _lastClientX = null;
            _lastClientY = null;
        }

        public override void Dispose()
        {
            DiagramManager.MouseDown -= DiagramManager_MouseDown;
            DiagramManager.MouseMove -= DiagramManager_MouseMove;
            DiagramManager.MouseUp -= DiagramManager_MouseUp;
        }
    }
}
