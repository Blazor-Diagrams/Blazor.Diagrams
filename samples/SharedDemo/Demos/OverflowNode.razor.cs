using Blazor.Diagrams;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Geometry;
using Microsoft.AspNetCore.Components;

namespace SharedDemo.Demos;

public class OverflowNodeComponent : ComponentBase
{
    protected readonly BlazorDiagram BlazorDiagram = new BlazorDiagram();

    protected override void OnInitialized()
    {
        base.OnInitialized();

        BlazorDiagram.RegisterComponent<NodeModel, OverflowNodeWidget>();

        var node1 = new NodeModel(new Point(50, 50))
        {
            Title = "Scrollable Node",
            ConsumeWheel = true // Feature demonstration
        };
        node1.AddPort(PortAlignment.Right);
        
        var node2 = new NodeModel(new Point(400, 100))
        {
            Title = "Another Node"
        };
        node2.AddPort(PortAlignment.Left);

        BlazorDiagram.Nodes.Add(new[] { node1, node2 });
        BlazorDiagram.Links.Add(new LinkModel(node1.GetPort(PortAlignment.Right), node2.GetPort(PortAlignment.Left)));
    }
}
