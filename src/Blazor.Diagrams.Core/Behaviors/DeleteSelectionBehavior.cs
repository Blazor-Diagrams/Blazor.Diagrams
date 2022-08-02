using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;
using Blazor.Diagrams.Core.Events;
using System.Linq;

namespace Blazor.Diagrams.Core.Behaviors
{
    public class DeleteSelectionBehavior : Behavior
    {
        public DeleteSelectionBehavior(DiagramBase diagram) : base(diagram)
        {
            Diagram.KeyDown += OnKeyDown;
        }

        private async void OnKeyDown(KeyboardEventArgs e)
        {
            if (e.AltKey || e.CtrlKey || e.ShiftKey || e.Code != Diagram.Options.DeleteKey)
                return;

            var wasSuspended = Diagram.SuspendRefresh;
            if (!wasSuspended) Diagram.SuspendRefresh = true;

            foreach (var sm in Diagram.GetSelectedModels().ToList())
            {
                if (sm.Locked)
                    continue;

                if (sm is GroupModel group && (await Diagram.Options.Constraints.ShouldDeleteGroup(group)))
                {
                    Diagram.RemoveGroup(group);
                }
                else if (sm is NodeModel node && (await Diagram.Options.Constraints.ShouldDeleteNode(node)))
                {
                    Diagram.Nodes.Remove(node);
                }
                else if (sm is BaseLinkModel link && (await Diagram.Options.Constraints.ShouldDeleteLink(link)))
                {
                    Diagram.Links.Remove(link);
                }
            }

            if (!wasSuspended)
            {
                Diagram.SuspendRefresh = false;
                Diagram.Refresh();
            }
        }

        public override void Dispose()
        {
            Diagram.KeyDown -= OnKeyDown;
        }
    }
}
