using Blazor.Diagrams;
using Blazor.Diagrams.Core.Anchors;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Linq;

namespace SharedDemo.Demos;

public class DynamicInsertionsComponent : ComponentBase
{
    private static readonly Random _random = new Random();
    protected readonly BlazorDiagram BlazorDiagram = new BlazorDiagram();

    protected override void OnInitialized()
    {
        base.OnInitialized();

        BlazorDiagram.Options.Groups.Enabled = true;
        BlazorDiagram.Nodes.Add(new NodeModel(new Point(300, 50)));
        BlazorDiagram.Nodes.Add(new NodeModel(new Point(300, 400)));

        BlazorDiagram.Options.Links.Factory = (d, s, ta) =>
        {
            var link = new LinkModel(new SinglePortAnchor(s as PortModel)
            {
                UseShapeAndAlignment = false
            }, ta)
            {
                SourceMarker = LinkMarker.Arrow
            };
            return link;
        };
    }

    protected void AddNode()
    {
        var x = _random.Next(0, (int)BlazorDiagram.Container.Width - 120);
        var y = _random.Next(0, (int)BlazorDiagram.Container.Height - 100);
        BlazorDiagram.Nodes.Add(new NodeModel(new Point(x, y)));
    }

    protected void RemoveNode()
    {
        var i = _random.Next(0, BlazorDiagram.Nodes.Count);
        BlazorDiagram.Nodes.Remove(BlazorDiagram.Nodes[i]);
    }

    protected void AddPort()
    {
        var node = BlazorDiagram.Nodes.FirstOrDefault(n => n.Selected);
        if (node == null)
            return;

        foreach (PortAlignment portAlignment in Enum.GetValues(typeof(PortAlignment)))
        {
            if (node.GetPort(portAlignment) == null)
            {
                node.AddPort(portAlignment);
                node.Refresh();
                break;
            }
        }
    }

    protected void RemovePort()
    {
        var node = BlazorDiagram.Nodes.FirstOrDefault(n => n.Selected);
        if (node == null)
            return;

        if (node.Ports.Count == 0)
            return;

        var i = _random.Next(0, node.Ports.Count);
        var port = node.Ports[i];

        BlazorDiagram.Links.Remove(port.Links.ToArray());
        node.RemovePort(port);
        node.Refresh();
    }

    protected void AddLink()
    {
        var selectedNodes = BlazorDiagram.Nodes.Where(n => n.Selected).ToArray();
        if (selectedNodes.Length != 2)
            return;

        var node1 = selectedNodes[0];
        var node2 = selectedNodes[1];

        if (node1 == null || node1.Ports.Count == 0 || node2 == null || node2.Ports.Count == 0)
            return;

        var sourcePort = node1.Ports[_random.Next(0, node1.Ports.Count)];
        var targetPort = node2.Ports[_random.Next(0, node2.Ports.Count)];
        BlazorDiagram.Links.Add(new LinkModel(sourcePort, targetPort));
    }
}
