using Blazor.Diagrams.Core;
using Blazor.Diagrams.Core.Models;
using Microsoft.AspNetCore.Components;

namespace SharedDemo.Demos
{
    public class CustomNodeComponent : ComponentBase
    {
        protected readonly DiagramManager diagramManager = new DiagramManager();

        protected override void OnInitialized()
        {
            base.OnInitialized();

            diagramManager.RegisterModelComponent<BotAnswerNode, BotAnswerWidget>();
            diagramManager.AddNode(NewNode(100, 100));
            diagramManager.AddNode(NewNode(300, 300));

            var node = new NodeModel(new Point(20, 20));
            node.AddPort(PortAlignment.TOP);
            node.AddPort(PortAlignment.RIGHT);
            node.AddPort(PortAlignment.BOTTOM);
            node.AddPort(PortAlignment.LEFT);
            diagramManager.AddNode(node);
        }

        private BotAnswerNode NewNode(double x, double y)
        {
            var node = new BotAnswerNode(new Point(x, y));
            node.AddPort(PortAlignment.TOP);
            node.AddPort(PortAlignment.BOTTOM);
            return node;
        }
    }
}
