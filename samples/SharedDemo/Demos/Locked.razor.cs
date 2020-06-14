using Blazor.Diagrams.Core;
using Blazor.Diagrams.Core.Models;
using Microsoft.AspNetCore.Components;

namespace SharedDemo
{
    public class LockedComponent : ComponentBase
    {
        protected readonly DiagramManager diagramManager = new DiagramManager();

        protected override void OnInitialized()
        {
            base.OnInitialized();

            var node1 = NewNode(50, 50);
            var node2 = NewNode(300, 300);
            diagramManager.AddNode(node1);
            diagramManager.AddNode(node2);
            diagramManager.AddNode(NewNode(300, 50));

            var link = diagramManager.AddLink(node1.GetPort(PortAlignment.RIGHT), node2.GetPort(PortAlignment.LEFT));
            link.Locked = true;
        }

        private NodeModel NewNode(double x, double y)
        {
            var node = new NodeModel(new Point(x, y));
            node.AddPort(PortAlignment.BOTTOM);
            node.AddPort(PortAlignment.TOP).Locked = true;
            node.AddPort(PortAlignment.LEFT);
            node.AddPort(PortAlignment.RIGHT);
            node.Locked = true;
            return node;
        }
    }
}
