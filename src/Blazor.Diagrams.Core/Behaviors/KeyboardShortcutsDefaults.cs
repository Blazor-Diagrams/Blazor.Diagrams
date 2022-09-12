using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;
using System.Linq;
using System.Threading.Tasks;

namespace Blazor.Diagrams.Core.Behaviors
{
    public static class KeyboardShortcutsDefaults
    {
        public static async ValueTask DeleteSelection(Diagram diagram)
        {
            var wasSuspended = diagram.SuspendRefresh;
            if (!wasSuspended) diagram.SuspendRefresh = true;

            //Group Delete
            var smGroup = diagram.GetSelectedModels().Where(d => d is GroupModel && !d.Locked).Select(x => x as GroupModel).ToArray();
            if (smGroup != null)
            {
                var result = await diagram.Options.Constraints.ShouldDeleteGroup(smGroup);
                if (result)
                {
                    foreach (var group in smGroup)
                    {
                        diagram.RemoveGroup(group);
                    }
                }
            }

            //Node Delete
            var smNodes = diagram.GetSelectedModels().Where(d => d is NodeModel && !d.Locked).Select(x => x as NodeModel).ToArray();
            if (smNodes != null)
            {
                var result = await diagram.Options.Constraints.ShouldDeleteNode(smNodes);
                if (result)
                {
                    diagram.Nodes.Remove(smNodes);
                }
            }

            //Node Links
            var smLinks = diagram.GetSelectedModels().Where(d => d is BaseLinkModel && !d.Locked).Select(x => x as BaseLinkModel).ToArray();
            if (smLinks != null)
            {
                var result = await diagram.Options.Constraints.ShouldDeleteLink(smLinks);
                if (result)
                {
                    diagram.Links.Remove(smLinks);
                }
            }

            if (!wasSuspended)
            {
                diagram.SuspendRefresh = false;
                diagram.Refresh();
            }
        }

        public static ValueTask Grouping(Diagram diagram)
        {
            if (!diagram.Options.Groups.Enabled)
                return ValueTask.CompletedTask;

            if (!diagram.GetSelectedModels().Any())
                return ValueTask.CompletedTask;

            var selectedNodes = diagram.Nodes.Where(n => n.Selected).ToArray();
            var nodesWithGroup = selectedNodes.Where(n => n.Group != null).ToArray();
            if (nodesWithGroup.Length > 0)
            {
                // Ungroup
                foreach (var group in nodesWithGroup.GroupBy(n => n.Group!).Select(g => g.Key))
                {
                    diagram.Ungroup(group);
                }
            }
            else
            {
                // Group
                if (selectedNodes.Length < 2)
                    return ValueTask.CompletedTask;

                if (selectedNodes.Any(n => n.Group != null))
                    return ValueTask.CompletedTask;

                diagram.Group(selectedNodes);
            }

            return ValueTask.CompletedTask;
        }
    }
}
