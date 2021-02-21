using Microsoft.AspNetCore.Components.Web;
using System;
using System.Linq;

namespace Blazor.Diagrams.Core.Behaviors
{
    public class GroupingBehavior : Behavior
    {
        public GroupingBehavior(DiagramManager diagramManager) : base(diagramManager)
        {
            DiagramManager.KeyDown += DiagramManager_KeyDown;
        }

        private void DiagramManager_KeyDown(KeyboardEventArgs e)
        {
            if (!DiagramManager.Options.Groups.Enabled)
                return;

            if (!DiagramManager.GetSelectedModels().Any())
                return;

            Console.WriteLine(DiagramManager.Options.Groups.KeyboardShortcut(e));
            if (!DiagramManager.Options.Groups.KeyboardShortcut(e))
                return;

            var selectedNodes = DiagramManager.Nodes.Where(n => n.Selected).ToArray();
            var nodesWithGroup = selectedNodes.Where(n => n.Group != null).ToArray();
            if (nodesWithGroup.Length > 0)
            {
                // Ungroup
                foreach (var group in nodesWithGroup.GroupBy(n => n.Group!).Select(g => g.Key))
                {
                    DiagramManager.Ungroup(group);
                }
            }
            else
            {
                // Group
                if (selectedNodes.Length < 2)
                    return;

                if (selectedNodes.Any(n => n.Group != null))
                    return;

                if (selectedNodes.Select(n => n.Layer).Distinct().Count() > 1)
                    return;

                DiagramManager.Group(selectedNodes);
            }
        }

        public override void Dispose()
        {
            DiagramManager.KeyDown -= DiagramManager_KeyDown;
        }
    }
}
