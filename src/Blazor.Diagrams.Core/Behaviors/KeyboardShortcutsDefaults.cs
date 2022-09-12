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

            bool deleteNodeFunc = false, deleteLinkFunc = false, deleteGroupFunc = false;
            bool isShouldDeleteNode = true, isShouldDeleteLink = true, isShouldDeleteGroup = true;

            foreach (var sm in diagram.GetSelectedModels().ToArray())
            {
                if (sm.Locked)
                    continue;

                if (sm is GroupModel group && isShouldDeleteGroup)
                {
                    if (!deleteGroupFunc)
                    {
                        isShouldDeleteGroup = await diagram.Options.Constraints.ShouldDeleteGroup(group);
                        deleteGroupFunc = true;
                        if (!isShouldDeleteGroup)
                            continue;
                    }
                    diagram.RemoveGroup(group);
                }
                else if (sm is NodeModel node && isShouldDeleteNode)
                {
                    if (!deleteNodeFunc)
                    {
                        isShouldDeleteNode = await diagram.Options.Constraints.ShouldDeleteNode(node);
                        deleteNodeFunc = true;
                        if (!isShouldDeleteNode)
                            continue;
                    }
                    diagram.Nodes.Remove(node);
                }
                else if (sm is BaseLinkModel link && isShouldDeleteLink)
                {
                    if (!deleteLinkFunc)
                    {
                        isShouldDeleteLink = await diagram.Options.Constraints.ShouldDeleteLink(link);
                        deleteLinkFunc = true;
                        if (!isShouldDeleteLink)
                            continue;
                    }
                    diagram.Links.Remove(link);
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
