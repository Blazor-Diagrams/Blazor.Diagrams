using Blazor.Diagrams.Core;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Core;
using Microsoft.AspNetCore.Components;

namespace SharedDemo.Demos
{
    public class GroupingComponent : ComponentBase
    {
        protected readonly DiagramManager diagramManager = new DiagramManager();

        protected override void OnInitialized()
        {
            base.OnInitialized();

            diagramManager.Options.Groups.Enabled = true;
            var node1 = NewNode(50, 50);
            var node2 = NewNode(250, 250);
            var node3 = NewNode(500, 100);
            diagramManager.Nodes.Add(node1, node2, node3);

            var group = diagramManager.Group(node1, node2);
            group.AddPort(PortAlignment.Bottom);
            group.AddPort(PortAlignment.Top);
            group.AddPort(PortAlignment.Left);
            group.AddPort(PortAlignment.Right);

            diagramManager.Links.Add(new LinkModel(node1.GetPort(PortAlignment.Right), node2.GetPort(PortAlignment.Left)));
            diagramManager.Links.Add(new LinkModel(group.GetPort(PortAlignment.Right), node3.GetPort(PortAlignment.Left)));
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
