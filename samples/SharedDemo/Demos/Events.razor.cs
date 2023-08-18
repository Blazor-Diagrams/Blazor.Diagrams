using Blazor.Diagrams;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace SharedDemo.Demos;

public class EventsComponent : ComponentBase
{
    protected readonly BlazorDiagram BlazorDiagram = new BlazorDiagram();
    protected readonly List<string> events = new List<string>();

    protected override void OnInitialized()
    {
        base.OnInitialized();

        RegisterEvents();

        var node1 = NewNode(50, 50);
        var node2 = NewNode(300, 300);
        BlazorDiagram.Nodes.Add(new[] { node1, node2, NewNode(300, 50) });
        BlazorDiagram.Links.Add(new LinkModel(node1.GetPort(PortAlignment.Right), node2.GetPort(PortAlignment.Left)));
    }

    private void RegisterEvents()
    {
        BlazorDiagram.Changed += () =>
        {
            events.Add("Changed");
            StateHasChanged();
        };

        BlazorDiagram.Nodes.Added += (n) => events.Add($"NodesAdded, NodeId={n.Id}");
        BlazorDiagram.Nodes.Removed += (n) => events.Add($"NodesRemoved, NodeId={n.Id}");

        BlazorDiagram.SelectionChanged += (m) =>
        {
            events.Add($"SelectionChanged, Id={m.Id}, Type={m.GetType().Name}, Selected={m.Selected}");
            StateHasChanged();
        };

        BlazorDiagram.Links.Added += (l) => events.Add($"Links.Added, LinkId={l.Id}");

        BlazorDiagram.Links.Removed += (l) => events.Add($"Links.Removed, LinkId={l.Id}");

        BlazorDiagram.PointerDown += (m, e) =>
        {
            events.Add($"MouseDown, Type={m?.GetType().Name}, ModelId={m?.Id}");
            StateHasChanged();
        };

        BlazorDiagram.PointerUp += (m, e) =>
        {
            events.Add($"MouseUp, Type={m?.GetType().Name}, ModelId={m?.Id}");
            StateHasChanged();
        };

        BlazorDiagram.PointerClick += (m, e) =>
        {
            events.Add($"MouseClick, Type={m?.GetType().Name}, ModelId={m?.Id}");
            StateHasChanged();
        };

        BlazorDiagram.PointerDoubleClick += (m, e) =>
        {
            events.Add($"MouseDoubleClick, Type={m?.GetType().Name}, ModelId={m?.Id}");
            StateHasChanged();
        };
    }

    private NodeModel NewNode(double x, double y)
    {
        var node = new NodeModel(new Point(x, y));
        node.AddPort(PortAlignment.Bottom);
        node.AddPort(PortAlignment.Top);
        node.AddPort(PortAlignment.Left);
        node.AddPort(PortAlignment.Right);
        node.Moved += (m) => events.Add($"Node.Moved, NodeId={node.Id}");
        return node;
    }
}
