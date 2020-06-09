using Microsoft.AspNetCore.Components.Web;
using System;

namespace Blazor.Diagrams.Core.Default
{
    public class ZoomSubManager : DiagramSubManager
    {
        public ZoomSubManager(DiagramManager diagramManager) : base(diagramManager)
        {
            DiagramManager.Wheel += DiagramManager_Wheel;
        }

        private void DiagramManager_Wheel(WheelEventArgs e)
        {
            var oldZoom = DiagramManager.Zoom;
            var sign = Math.Sign(e.DeltaY);
            var newZoom = oldZoom + 0.05f * sign;

            if (newZoom < 0)
                return;

            var scale = newZoom - oldZoom;
            var x = e.ClientX - DiagramManager.Container.Left;
            var y = e.ClientY - DiagramManager.Container.Top;
            DiagramManager.Pan = DiagramManager.Pan.Add(scale * x, scale * y);

            DiagramManager.Zoom = newZoom;
            DiagramManager.Refresh();
        }

        public override void Dispose()
        {
            DiagramManager.Wheel -= DiagramManager_Wheel;
        }
    }
}
