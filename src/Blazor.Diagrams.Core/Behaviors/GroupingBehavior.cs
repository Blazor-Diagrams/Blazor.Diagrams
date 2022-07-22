using Blazor.Diagrams.Core.Events;
using System;
using System.Linq;

namespace Blazor.Diagrams.Core.Behaviors
{
    public class GroupingBehavior : Behavior
    {
        public GroupingBehavior(DiagramBase diagram) : base(diagram)
        {
            Diagram.KeyDown += Diagram_KeyDown;
        }

        private void Diagram_KeyDown(KeyboardEventArgs e)
        {
            if (!Diagram.Options.Groups.Enabled)
                return;

            if (!Diagram.GetSelectedModels().Any())
                return;

            if (!Diagram.Options.Groups.KeyboardShortcut(e))
                return;

            var selectedNodes = Diagram.Nodes.Where(n => n.Selected).ToArray();
            var nodesWithGroup = selectedNodes.Where(n => n.Group != null).ToArray();
            if (nodesWithGroup.Length > 0)
            {
                // Ungroup
                foreach (var group in nodesWithGroup.GroupBy(n => n.Group!).Select(g => g.Key))
                {
                    Diagram.Ungroup(group);
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

                Diagram.Group(selectedNodes);
            }
        }

        public override void Dispose()
        {
            Diagram.KeyDown -= Diagram_KeyDown;
        }
    }
}
