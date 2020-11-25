using Blazor.Diagrams.Core;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Core;
using Microsoft.AspNetCore.Components;

namespace SharedDemo.Demos
{
    public class GroupingComponent : ComponentBase
    {
        protected readonly DiagramManager diagramManager = new DiagramManager(new DiagramOptions
        {
            GroupingEnabled = true
        });

        protected override void OnInitialized()
        {
            base.OnInitialized();

            var node1 = NewNode(50, 50);
            var node2 = NewNode(200, 250);
            var node3 = NewNode(400, 100);

            diagramManager.AddLink(node1.GetPort(PortAlignment.Right), node2.GetPort(PortAlignment.Left));
            diagramManager.AddLink(node2.GetPort(PortAlignment.Right), node3.GetPort(PortAlignment.Left));
            diagramManager.Group(node1, node2);
            diagramManager.AddNode(node3);
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
