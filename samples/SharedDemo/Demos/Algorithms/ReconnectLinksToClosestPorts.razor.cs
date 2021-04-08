using Blazor.Diagrams.Algorithms;
using Blazor.Diagrams.Core;
using Blazor.Diagrams.Core.Models;
using Microsoft.AspNetCore.Components;
using Blazor.Diagrams.Core.Geometry;

namespace SharedDemo.Demos.Algorithms
{
    public class ReconnectLinksToClosestPortsComponent : ComponentBase
    {
        protected readonly Diagram diagram = new Diagram();

        protected override void OnInitialized()
        {
            base.OnInitialized();

            var node1 = NewNode(50, 50);
            var node2 = NewNode(300, 300);
            var node3 = NewNode(300, 50);
            diagram.Links.Add(new LinkModel(node1.GetPort(PortAlignment.Top), node2.GetPort(PortAlignment.Right)));
            diagram.Links.Add(new LinkModel(node1.GetPort(PortAlignment.Bottom), node3.GetPort(PortAlignment.Top)));
            diagram.Nodes.Add(new[] { node1, node2, node3 });
        }


        protected void ReconnectLinks() => diagram.ReconnectLinksToClosestPorts();


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
