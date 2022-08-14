using Blazor.Diagrams;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Microsoft.AspNetCore.Components;

namespace SharedDemo.Demos
{
    public class GroupingComponent : ComponentBase
    {
        protected readonly Diagram diagram = new Diagram();

        protected override void OnInitialized()
        {
            base.OnInitialized();

            diagram.Options.Groups.Enabled = true;
            diagram.Options.LinksLayerOrder = 2;
            diagram.Options.NodesLayerOrder = 1;
            var node1 = NewNode(50, 50);
            var node2 = NewNode(250, 250);
            var node3 = NewNode(500, 100);
            var node4 = NewNode(700, 350);
            diagram.Nodes.Add(new[] { node1, node2, node3, node4 });

            diagram.Links.Add(new LinkModel(node1.GetPort(PortAlignment.Right), node2.GetPort(PortAlignment.Left)));
            diagram.Links.Add(new LinkModel(node2.GetPort(PortAlignment.Right), node3.GetPort(PortAlignment.Left)));
            diagram.Links.Add(new LinkModel(node1.GetPort(PortAlignment.Right), node3.GetPort(PortAlignment.Left)));

            var group1 = diagram.Group(node1, node2);
            var group2 = diagram.Group(group1, node3);

            diagram.Links.Add(new LinkModel(group2, node4));
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
