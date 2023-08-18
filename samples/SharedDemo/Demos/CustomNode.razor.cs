using Blazor.Diagrams.Core.Models;
using Microsoft.AspNetCore.Components;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams;

namespace SharedDemo.Demos;

public class CustomNodeComponent : ComponentBase
{
    protected readonly BlazorDiagram BlazorDiagram = new BlazorDiagram();

    protected override void OnInitialized()
    {
        base.OnInitialized();

        BlazorDiagram.RegisterComponent<BotAnswerNode, BotAnswerWidget>();

        var node = new NodeModel(new Point(20, 20));
        node.AddPort(PortAlignment.Top);
        node.AddPort(PortAlignment.Right);
        node.AddPort(PortAlignment.Bottom);
        node.AddPort(PortAlignment.Left);

        BlazorDiagram.Nodes.Add(new[] { node, NewNode(100, 100), NewNode(300, 300) });
    }

    private BotAnswerNode NewNode(double x, double y)
    {
        var node = new BotAnswerNode(new Point(x, y));
        node.AddPort(PortAlignment.Top);
        node.AddPort(PortAlignment.Bottom);
        return node;
    }
}
