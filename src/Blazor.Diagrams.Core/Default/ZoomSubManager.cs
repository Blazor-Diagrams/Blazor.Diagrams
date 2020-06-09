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
            var sign = Math.Sign(e.DeltaY);
            DiagramManager.Zoom += 0.05f* sign;
            DiagramManager.Refresh();
        }

        public override void Dispose()
        {
            DiagramManager.Wheel -= DiagramManager_Wheel;
        }
    }
}
