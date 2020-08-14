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
                if (e.CtrlKey && sm.Selected)
                {
                    DiagramManager.UnselectModel(sm);
                }
                else if (!sm.Selected)
                {
                    DiagramManager.SelectModel(sm, !e.CtrlKey);
                }
            }
        }

        public override void Dispose()
        {
            DiagramManager.MouseDown -= DiagramManager_MouseDown;
        }
    }
}
