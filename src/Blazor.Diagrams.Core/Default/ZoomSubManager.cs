using Blazor.Diagrams.Core.Models;
using Microsoft.AspNetCore.Components.Web;

namespace Blazor.Diagrams.Core.Default
{
    public class ZoomSubManager : DiagramSubManager
    {
        private const float _scaleBy = 1.05f;

        public ZoomSubManager(DiagramManager diagramManager) : base(diagramManager)
        {
            DiagramManager.Wheel += DiagramManager_Wheel;
        }

        private void DiagramManager_Wheel(WheelEventArgs e)
        {
            var oldZoom = DiagramManager.Zoom;
            var newZoom = e.DeltaY > 0 ? oldZoom * _scaleBy : oldZoom / _scaleBy;

            if (newZoom < 0)
                return;

            var cuX = e.ClientX - DiagramManager.Container.Left;
            var cuY = e.ClientY - DiagramManager.Container.Top;
            var cX = DiagramManager.Pan.X + cuX / oldZoom;
            var cY = DiagramManager.Pan.Y + cuY / oldZoom;
            DiagramManager.Pan = new Point(cX - cuX / newZoom, cY - cuY / newZoom);

            DiagramManager.Zoom = newZoom;
            DiagramManager.Refresh();
        }

        public override void Dispose()
        {
            DiagramManager.Wheel -= DiagramManager_Wheel;
        }
    }
}
