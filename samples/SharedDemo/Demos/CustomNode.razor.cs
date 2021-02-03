using Blazor.Diagrams.Core;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Core;
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

            var node = new NodeModel(new Point(20, 20));
            node.AddPort(PortAlignment.Top);
            node.AddPort(PortAlignment.Right);
            node.AddPort(PortAlignment.Bottom);
            node.AddPort(PortAlignment.Left);

            diagramManager.Nodes.Add(node, NewNode(100, 100), NewNode(300, 300));
        }

        private BotAnswerNode NewNode(double x, double y)
        {
            var node = new BotAnswerNode(new Point(x, y));
            node.AddPort(PortAlignment.Top);
            node.AddPort(PortAlignment.Bottom);
            return node;
        }
    }
}
