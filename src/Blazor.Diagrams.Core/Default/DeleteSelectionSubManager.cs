using Blazor.Diagrams.Core.Models;
using Microsoft.AspNetCore.Components.Web;
using System.Linq;

namespace Blazor.Diagrams.Core.Default
{
    public class DeleteSelectionSubManager : DiagramSubManager
    {
        public DeleteSelectionSubManager(DiagramManager diagramManager) : base(diagramManager)
        {
            DiagramManager.KeyDown += DiagramManager_KeyDown;
        }

        private void DiagramManager_KeyDown(KeyboardEventArgs e)
        {
            if (e.AltKey || e.CtrlKey || e.ShiftKey || e.Code != DiagramManager.Options.DeleteKey)
                return;

            // TODO: BATCH
            foreach (var sm in DiagramManager.SelectedModels.ToList())
            {
                if (sm.Locked)
                    continue;

                if (sm is NodeModel node)
                {
                    DiagramManager.Nodes.Remove(node);
                }
                else if (sm is LinkModel link)
                {
                    DiagramManager.Links.Remove(link);
                }
            }
        }

        public override void Dispose()
        {
            DiagramManager.KeyDown -= DiagramManager_KeyDown;
        }
    }
}
