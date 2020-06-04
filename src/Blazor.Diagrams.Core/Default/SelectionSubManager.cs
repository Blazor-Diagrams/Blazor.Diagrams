using Blazor.Diagrams.Core.Models.Base;
using Microsoft.AspNetCore.Components.Web;

namespace Blazor.Diagrams.Core.Default
{
    public class SelectionSubManager : DiagramSubManager
    {
        public SelectionSubManager(DiagramManager diagramManager) : base(diagramManager)
        {
            DiagramManager.MouseDown += DiagramManager_MouseDown;
        }

        private void DiagramManager_MouseDown(Model model, MouseEventArgs e)
        {
            if (model == null)
            {
                DiagramManager.UnselectAll();
            }
            else if (model is SelectableModel sm)
            {
                DiagramManager.SelectModel(sm, e.CtrlKey == false);
            }
        }

        public override void Dispose()
        {
            DiagramManager.MouseDown -= DiagramManager_MouseDown;
        }
    }
}
