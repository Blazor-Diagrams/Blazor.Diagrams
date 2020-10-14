using Blazor.Diagrams.Core.Models.Base;
using Blazor.Diagrams.Core.Models.Core;
using Microsoft.AspNetCore.Components.Web;

namespace Blazor.Diagrams.Core.Default
{
    public class PanSubManager : DiagramSubManager
    {
        private Point? _initialPan;
        private double _lastClientX;
        private double _lastClientY;

        public PanSubManager(DiagramManager diagramManager) : base(diagramManager)
        {
            DiagramManager.MouseDown += DiagramManager_MouseDown;
            DiagramManager.MouseMove += DiagramManager_MouseMove;
            DiagramManager.MouseUp += DiagramManager_MouseUp;
        }

        private void DiagramManager_MouseDown(Model model, MouseEventArgs e)
        {
            if (model != null)
                return;

            _initialPan = DiagramManager.Pan;
            _lastClientX = e.ClientX;
            _lastClientY = e.ClientY;
        }

        private void DiagramManager_MouseMove(Model model, MouseEventArgs e)
        {
            if (_initialPan == null)
                return;

            var deltaX = e.ClientX - _lastClientX - (DiagramManager.Pan.X - _initialPan.X);
            var deltaY = e.ClientY - _lastClientY - (DiagramManager.Pan.Y - _initialPan.Y);
            DiagramManager.ChangePan(deltaX, deltaY);
        }

        private void DiagramManager_MouseUp(Model model, MouseEventArgs e)
        {
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
