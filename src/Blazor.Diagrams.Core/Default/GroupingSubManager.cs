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
            if (DiagramManager.SelectedModels.Count == 0)
                return;

            if (e.CtrlKey && e.Key.Equals("g", StringComparison.InvariantCultureIgnoreCase))
            {
                if (e.ShiftKey)
                {
                    // Ungroup, todo
                }
                else
                {
                    // Group
                    var selectedNodes = DiagramManager.SelectedModels
                        .Where(m => m is NodeModel)
                        .Select(m => (NodeModel)m)
                        .ToArray();

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
