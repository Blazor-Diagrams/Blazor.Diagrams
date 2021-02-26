using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;
using Microsoft.AspNetCore.Components.Web;
using System.Linq;

namespace Blazor.Diagrams.Core.Behaviors
{
    public class DeleteSelectionBehavior : Behavior
    {
        public DeleteSelectionBehavior(Diagram diagram) : base(diagram)
        {
            Diagram.KeyDown += Diagram_KeyDown;
        }

        private void Diagram_KeyDown(KeyboardEventArgs e)
        {
            if (e.AltKey || e.CtrlKey || e.ShiftKey || e.Code != Diagram.Options.DeleteKey)
                return;

            // TODO: BATCH REFRESH
            foreach (var sm in Diagram.GetSelectedModels().ToList())
            {
                if (sm.Locked)
                    continue;

                if (sm is GroupModel group)
                {
                    Diagram.RemoveGroup(group);
                }
                else if (sm is NodeModel node)
                {
                    Diagram.Nodes.Remove(node);
                }
                else if (sm is BaseLinkModel link)
                {
                    Diagram.Links.Remove(link);
                }
            }
        }

        public override void Dispose()
        {
            Diagram.KeyDown -= Diagram_KeyDown;
        }
    }
}
