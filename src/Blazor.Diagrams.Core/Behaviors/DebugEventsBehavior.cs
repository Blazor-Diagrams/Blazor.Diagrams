using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;
using System;
using System.Linq;

namespace Blazor.Diagrams.Core.Behaviors
{
    public class DebugEventsBehavior : Behavior
    {
        public DebugEventsBehavior(DiagramManager diagramManager) : base(diagramManager)
        {
            DiagramManager.Changed += DiagramManager_Changed;
            DiagramManager.ContainerChanged += DiagramManager_ContainerChanged;
            DiagramManager.PanChanged += DiagramManager_PanChanged;
            DiagramManager.Nodes.Added += Nodes_Added;
            DiagramManager.Nodes.Removed += Nodes_Removed;
            DiagramManager.Links.Added += Links_Added;
            DiagramManager.Links.Removed += Links_Removed;
            DiagramManager.GroupAdded += DiagramManager_GroupAdded;
            DiagramManager.GroupRemoved += DiagramManager_GroupRemoved;
            DiagramManager.GroupUngrouped += DiagramManager_GroupUngrouped;
            DiagramManager.SelectionChanged += DiagramManager_SelectionChanged;
            DiagramManager.ZoomChanged += DiagramManager_ZoomChanged;
        }

        private void DiagramManager_ZoomChanged()
        {
            Console.WriteLine($"ZoomChanged, Zoom={DiagramManager.Zoom}");
        }

        private void DiagramManager_SelectionChanged(SelectableModel obj)
        {
            Console.WriteLine($"SelectionChanged, Model={obj.GetType().Name}, Selected={obj.Selected}");
        }

        private void DiagramManager_GroupUngrouped(GroupModel obj)
        {
            Console.WriteLine($"GroupUngrouped, Id={obj.Id}");
        }

        private void Links_Removed(BaseLinkModel[] obj)
        {
            Console.WriteLine($"Links.Removed, Links=[{string.Join(", ", obj.Select(x => x.Id))}]");
        }

        private void Links_Added(BaseLinkModel[] obj)
        {
            Console.WriteLine($"Links.Added, Links=[{string.Join(", ", obj.Select(x => x.Id))}]");
        }

        private void Nodes_Removed(NodeModel[] obj)
        {
            Console.WriteLine($"Nodes.Removed, Nodes=[{string.Join(", ", obj.Select(x => x.Id))}]");
        }

        private void Nodes_Added(NodeModel[] obj)
        {
            Console.WriteLine($"Nodes.Added, Nodes=[{string.Join(", ", obj.Select(x => x.Id))}]");
        }

        private void DiagramManager_PanChanged()
        {
            Console.WriteLine($"PanChanged, Pan={DiagramManager.Pan}");
        }

        private void DiagramManager_GroupRemoved(GroupModel obj)
        {
            Console.WriteLine($"GroupRemoved, Id={obj.Id}");
        }

        private void DiagramManager_GroupAdded(GroupModel obj)
        {
            Console.WriteLine($"GroupAdded, Id={obj.Id}");
        }

        private void DiagramManager_ContainerChanged()
        {
            Console.WriteLine($"ContainerChanged, Container={DiagramManager.Container}");
        }

        private void DiagramManager_Changed()
        {
            Console.WriteLine("Changed");
        }

        public override void Dispose()
        {
            DiagramManager.Changed -= DiagramManager_Changed;
            DiagramManager.ContainerChanged -= DiagramManager_ContainerChanged;
            DiagramManager.PanChanged -= DiagramManager_PanChanged;
            DiagramManager.Nodes.Added -= Nodes_Added;
            DiagramManager.Nodes.Removed -= Nodes_Removed;
            DiagramManager.Links.Added -= Links_Added;
            DiagramManager.Links.Removed -= Links_Removed;
            DiagramManager.GroupAdded -= DiagramManager_GroupAdded;
            DiagramManager.GroupRemoved -= DiagramManager_GroupRemoved;
            DiagramManager.GroupUngrouped -= DiagramManager_GroupUngrouped;
            DiagramManager.SelectionChanged -= DiagramManager_SelectionChanged;
            DiagramManager.ZoomChanged -= DiagramManager_ZoomChanged;
        }
    }
}
