using Blazor.Diagrams.Core.Models.Base;
using Blazor.Diagrams.Core.Models.Core;
using Microsoft.AspNetCore.Components.Web;
using System;

namespace Blazor.Diagrams.Core.Behaviors
{
    public class PanBehavior : Behavior
    {
        private Point? _initialPan;
        private double _lastClientX;
        private double _lastClientY;

        public PanBehavior(DiagramManager diagramManager) : base(diagramManager)
        {
            DiagramManager.MouseDown += DiagramManager_MouseDown;
            DiagramManager.MouseMove += DiagramManager_MouseMove;
            DiagramManager.MouseUp += DiagramManager_MouseUp;
        }

        private void DiagramManager_MouseDown(Model model, MouseEventArgs e)
        {
            if (!DiagramManager.Options.AllowPanning || model != null || e.ShiftKey)
                return;

            _initialPan = DiagramManager.Pan;
            _lastClientX = e.ClientX;
            _lastClientY = e.ClientY;
        }

        private void DiagramManager_MouseMove(Model model, MouseEventArgs e)
        {
            if (!DiagramManager.Options.AllowPanning || _initialPan == null)
                return;

            var deltaX = e.ClientX - _lastClientX - (DiagramManager.Pan.X - _initialPan.X);
            var deltaY = e.ClientY - _lastClientY - (DiagramManager.Pan.Y - _initialPan.Y);
            DiagramManager.UpdatePan(deltaX, deltaY);
        }

        private void DiagramManager_MouseUp(Model model, MouseEventArgs e)
        {
            if (!DiagramManager.Options.AllowPanning)
                return;

            _initialPan = null;
        }

        public override void Dispose()
        {
            DiagramManager.MouseDown -= DiagramManager_MouseDown;
            DiagramManager.MouseMove -= DiagramManager_MouseMove;
            DiagramManager.MouseUp -= DiagramManager_MouseUp;
        }
    }
}
