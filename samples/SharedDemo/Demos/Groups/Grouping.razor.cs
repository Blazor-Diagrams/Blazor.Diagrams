using Blazor.Diagrams.Core;
using Blazor.Diagrams.Core.Models;
using Microsoft.AspNetCore.Components;
using Blazor.Diagrams.Core.Geometry;

namespace SharedDemo.Demos
{
    public class GroupingComponent : ComponentBase
    {
        protected readonly Diagram diagram = new Diagram();

        protected override void OnInitialized()
        {
            base.OnInitialized();

            diagram.Options.Groups.Enabled = true;
            var node1 = NewNode(50, 50);
            var node2 = NewNode(250, 250);
            var node3 = NewNode(500, 100);
            diagram.Nodes.Add(new[] { node1, node2, node3 });

            diagram.Links.Add(new LinkModel(node1.GetPort(PortAlignment.Right), node2.GetPort(PortAlignment.Left)));
        }

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
