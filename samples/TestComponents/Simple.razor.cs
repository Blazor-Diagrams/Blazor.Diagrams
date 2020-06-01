using Blazor.Diagrams.Core;
using Blazor.Diagrams.Core.Models;
using Microsoft.AspNetCore.Components;

namespace TestComponents
{
    public class SimpleComponent : ComponentBase
    {
        protected readonly DiagramManager diagramManager = new DiagramManager();

        protected override void OnInitialized()
        {
            base.OnInitialized();

            var node1 = NewNode(0, 0);
            var node2 = NewNode(300, 300);
            node1.GetPort(PortAlignment.RIGHT).AddLink(node2.GetPort(PortAlignment.LEFT));
            diagramManager.AddNode(node1);
            diagramManager.AddNode(node2);
        }

        private Node NewNode(double x, double y)
        {
            var node = new Node(new Point(x, y));
            node.AddPort(PortAlignment.BOTTOM);
            node.AddPort(PortAlignment.TOP);
            node.AddPort(PortAlignment.LEFT);
            node.AddPort(PortAlignment.RIGHT);
            return node;
        }
    }
}
