using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;
using Microsoft.AspNetCore.Components.Web;
using System.Linq;

namespace Blazor.Diagrams.Core.Behaviors
{
    public class DeleteSelectionBehavior : Behavior
    {
        public DeleteSelectionBehavior(DiagramManager diagramManager) : base(diagramManager)
        {
            DiagramManager.KeyDown += DiagramManager_KeyDown;
        }

        private void DiagramManager_KeyDown(KeyboardEventArgs e)
        {
            if (e.AltKey || e.CtrlKey || e.ShiftKey || e.Code != DiagramManager.Options.DeleteKey)
                return;

            // TODO: BATCH REFRESH
            foreach (var sm in DiagramManager.GetSelectedModels().ToList())
            {
                if (sm.Locked)
                    continue;

                if (sm is GroupModel group)
                {
                    DiagramManager.RemoveGroup(group);
                }
                else if (sm is NodeModel node)
                {
                    DiagramManager.Nodes.Remove(node);
                }
                else if (sm is BaseLinkModel link)
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
