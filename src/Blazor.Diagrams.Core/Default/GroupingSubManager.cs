using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Linq;

namespace Blazor.Diagrams.Core.Default
{
    public class GroupingSubManager : DiagramSubManager
    {
        public GroupingSubManager(DiagramManager diagramManager) : base(diagramManager)
        {
            DiagramManager.KeyDown += DiagramManager_KeyDown;
            DiagramManager.SelectionChanged += DiagramManager_SelectionChanged;
        }

        private void DiagramManager_SelectionChanged(SelectableModel model, bool selected)
        {
            if (!DiagramManager.Options.GroupingEnabled)
                return;

            if (!(model is NodeModel node))
                return;

            if (node.Group == null)
                return;

            foreach (var n in node.Group.Nodes)
            {
                if (n == node || n.Selected == selected)
                    continue;

                if (selected)
                {
                    DiagramManager.SelectModel(n, false);
                }
                else
                {
                    DiagramManager.UnselectModel(n);
                }
            }
        }

        private void DiagramManager_KeyDown(KeyboardEventArgs e)
        {
            if (!DiagramManager.Options.GroupingEnabled)
                return;

            if (DiagramManager.SelectedModels.Count == 0)
                return;

            if (e.CtrlKey && e.AltKey && e.Key.Equals("g", StringComparison.InvariantCultureIgnoreCase))
            {
                var selectedNodes = DiagramManager.SelectedModels
                    .Where(m => m is NodeModel)
                    .Select(m => (NodeModel)m)
                    .ToArray();

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
        }

        public override void Dispose()
        {
            DiagramManager.KeyDown -= DiagramManager_KeyDown;
        }
    }
}
