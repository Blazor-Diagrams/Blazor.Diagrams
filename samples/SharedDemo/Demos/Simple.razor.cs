using Blazor.Diagrams.Core;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Core;
using Microsoft.AspNetCore.Components;

namespace SharedDemo
{
    public class SimpleComponent : ComponentBase
    {
        protected readonly DiagramManager diagramManager = new DiagramManager();

        protected override void OnInitialized()
        {
            base.OnInitialized();

            var node1 = NewNode(50, 50);
            var node2 = NewNode(300, 300);
            diagramManager.Links.Add(new LinkModel(node1.GetPort(PortAlignment.Right), node2.GetPort(PortAlignment.Left)));
            diagramManager.Nodes.Add(node1, node2, NewNode(300, 50));
        }

        protected void ToggleZoom() => diagramManager.Options.Zoom.Enabled = !diagramManager.Options.Zoom.Enabled;

        protected void TogglePanning() => diagramManager.Options.AllowPanning = !diagramManager.Options.AllowPanning;

        protected void ToggleVirtualization()
            => diagramManager.Options.EnableVirtualization = !diagramManager.Options.EnableVirtualization;

        private NodeModel NewNode(double x, double y)
        {
            var node = new NodeModel(new Point(x, y));
            node.AddPort(PortAlignment.Bottom);
            node.AddPort(PortAlignment.Top);
            node.AddPort(PortAlignment.Left);
            node.AddPort(PortAlignment.Right);
            return node;
        }
    }
}
