using Blazor.Diagrams.Core;
using Blazor.Diagrams.Core.Models;
using Microsoft.AspNetCore.Components;
using Blazor.Diagrams.Core.Geometry;

namespace SharedDemo.Demos
{
    public class CustomNodeComponent : ComponentBase
    {
        protected readonly Diagram diagram = new Diagram();

        protected override void OnInitialized()
        {
            base.OnInitialized();

            diagram.RegisterModelComponent<BotAnswerNode, BotAnswerWidget>();

            var node = new NodeModel(new Point(20, 20));
            node.AddPort(PortAlignment.Top);
            node.AddPort(PortAlignment.Right);
            node.AddPort(PortAlignment.Bottom);
            node.AddPort(PortAlignment.Left);

            diagram.Nodes.Add(new[] { node, NewNode(100, 100), NewNode(300, 300) });
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
