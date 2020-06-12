using Blazor.Diagrams.Core.Extensions;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;
using Microsoft.AspNetCore.Components.Web;
using System.Linq;

namespace Blazor.Diagrams.Core.Default
{
    public class DragNodeSubManager : DiagramSubManager
    {
        private Point[]? _initialPositions;
        private double? _lastClientX;
        private double? _lastClientY;

        public DragNodeSubManager(DiagramManager diagramManager) : base(diagramManager)
        {
            DiagramManager.MouseDown += DiagramManager_MouseDown;
            DiagramManager.MouseMove += DiagramManager_MouseMove;
            DiagramManager.MouseUp += DiagramManager_MouseUp;
        }

        private void DiagramManager_MouseDown(Model model, MouseEventArgs e)
        {
            if (!(model is NodeModel))
                return;

            // Don't link this linq
            _initialPositions = DiagramManager.SelectedModels
                .Where(m => m is NodeModel)
                .Select(m => (m as NodeModel).Position)
                .ToArray();

            _lastClientX = e.ClientX;
            _lastClientY = e.ClientY;
        }

        private void DiagramManager_MouseMove(Model model, MouseEventArgs e)
        {
            if (_initialPositions == null || _lastClientX == null || _lastClientY == null)
                return;

            double deltaX = (e.ClientX - _lastClientX.Value) / DiagramManager.Zoom;
            double deltaY = (e.ClientY - _lastClientY.Value) / DiagramManager.Zoom;

            foreach ((var i, var sm) in DiagramManager.SelectedModels.LoopWithIndex())
            {
                if (!(sm is NodeModel node) || node.Locked)
                    continue;

                // The order shouldn't change between MouseDown & MouseMove
                var initialPosition = _initialPositions[i];
                node.UpdatePosition(deltaX - (node.Position.X - initialPosition.X),
                    deltaY - (node.Position.Y - initialPosition.Y));
                node.RefreshAll();
            }
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
