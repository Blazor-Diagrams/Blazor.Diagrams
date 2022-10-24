using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;
using Microsoft.AspNetCore.Components.Web;
using System.Linq;
using System.Threading.Tasks;

namespace Blazor.Diagrams.Core.Behaviors
{
    public class DeleteSelectionBehavior : Behavior
    {
        public DeleteSelectionBehavior(Diagram diagram) : base(diagram)
        {
            Diagram.KeyDown += Diagram_KeyDown;
        }

        private async void Diagram_KeyDown(KeyboardEventArgs e)
        {
            if (e.AltKey || e.CtrlKey || e.ShiftKey || e.Code != Diagram.Options.DeleteKey)
                return;

            Diagram.Batch(async () =>
            {
                    foreach (var sm in Diagram.GetSelectedModels().ToList())
                    {
                        if (sm.Locked)
                            continue;

                        if (sm is GroupModel group && await Diagram.Options.Constraints.ShouldDeleteGroup(group))
                        {
                            Diagram.RemoveGroup(group);
                        }
                        else if (sm is NodeModel node && await Diagram.Options.Constraints.ShouldDeleteNode(node))
                        {
                            Diagram.Nodes.Remove(node);
                        }
                        else if (sm is BaseLinkModel link && await Diagram.Options.Constraints.ShouldDeleteLink(link))
                        {
                            Diagram.Links.Remove(link);
                        }
                    }
            });

            await System.Threading.Tasks.Task.CompletedTask;
        }

        public override void Dispose()
        {
            Diagram.KeyDown -= Diagram_KeyDown;
        }
    }
}
