using Blazor.Diagrams;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Geometry;
using Microsoft.AspNetCore.Components;

namespace SharedDemo.Demos;

public class Issue574Component : ComponentBase
{
    protected readonly BlazorDiagram BlazorDiagram = new BlazorDiagram();

    protected override void OnInitialized()
    {
        base.OnInitialized();

        BlazorDiagram.RegisterComponent<NodeModel, Issue574Widget>();

        var node1 = new NodeModel(new Point(50, 50));
        node1.AddPort(PortAlignment.Right);
        
        var node2 = new NodeModel(new Point(400, 50))
        {
            ConsumeWheel = true
        };
        node2.AddPort(PortAlignment.Left);

        BlazorDiagram.Nodes.Add(new[] { node1, node2 });
    }
}
