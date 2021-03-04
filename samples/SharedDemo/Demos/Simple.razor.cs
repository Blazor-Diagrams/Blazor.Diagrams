using Blazor.Diagrams.Core;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Core;
using Microsoft.AspNetCore.Components;

namespace SharedDemo
{
    public class SimpleComponent : ComponentBase
    {
        protected readonly Diagram diagram = new Diagram();

        protected override void OnInitialized()
        {
            base.OnInitialized();

            var node1 = NewNode(50, 50);
            var node2 = NewNode(300, 300);
            diagram.Links.Add(new LinkModel(node1.GetPort(PortAlignment.Right), node2.GetPort(PortAlignment.Left)));
            diagram.Nodes.Add(new[] { node1, node2, NewNode(300, 50) });
        }

        protected void ToggleZoom() => diagram.Options.Zoom.Enabled = !diagram.Options.Zoom.Enabled;

        protected void TogglePanning() => diagram.Options.AllowPanning = !diagram.Options.AllowPanning;

        protected void ToggleVirtualization()
            => diagram.Options.EnableVirtualization = !diagram.Options.EnableVirtualization;

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
