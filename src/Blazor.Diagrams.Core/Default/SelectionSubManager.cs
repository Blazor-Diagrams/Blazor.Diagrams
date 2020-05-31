using Blazor.Diagrams.Core.Models;
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
                DiagramManager.UnselectNode();
            }
            else if (model is Node node)
            {
                DiagramManager.SelectNode(node);
            }
        }

        public override void Dispose()
        {
            DiagramManager.MouseDown -= DiagramManager_MouseDown;
        }
    }
}
