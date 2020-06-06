using Blazor.Diagrams.Core.Models;
using Microsoft.AspNetCore.Components.Web;

namespace Blazor.Diagrams.Core.Default
{
    public class DeleteSelectionSubManager : DiagramSubManager
    {
        private const string _deleteKeyCode = "Delete";

        public DeleteSelectionSubManager(DiagramManager diagramManager) : base(diagramManager)
        {
            DiagramManager.KeyDown += DiagramManager_KeyDown;
        }

        private void DiagramManager_KeyDown(KeyboardEventArgs e)
        {
            if (e.AltKey || e.CtrlKey || e.ShiftKey || e.Code != _deleteKeyCode)
                return;

            var somethingWasRemoved = false;

            foreach (var sm in DiagramManager.SelectedModels)
            {
                if (sm is NodeModel node)
                {
                    DiagramManager.RemoveNode(node, false);
                    somethingWasRemoved = true;
                }
                else if (sm is LinkModel link)
                {
                    DiagramManager.RemoveLink(link, false);
                    somethingWasRemoved = true;
                }
            }

            if (somethingWasRemoved)
            {
                DiagramManager.Refresh();
            }
        }

        public override void Dispose()
        {
            DiagramManager.KeyDown -= DiagramManager_KeyDown;
        }
    }
}
