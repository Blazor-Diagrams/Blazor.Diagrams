using Blazor.Diagrams;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using System;

namespace SharedDemo.Demos.Groups;

public partial class Dynamic
{
    private readonly BlazorDiagram _blazorDiagram = new BlazorDiagram();

    protected override void OnInitialized()
    {
        base.OnInitialized();

        LayoutData.Title = "Dynamic Groups";
        LayoutData.Info = "You can create and modify groups dynamically!";
        LayoutData.DataChanged();
        
        _blazorDiagram.Options.LinksLayerOrder = 2;
        _blazorDiagram.Options.NodesLayerOrder = 1;

        var node1 = NewNode(50, 150);
        var node2 = NewNode(250, 350);
        var node3 = NewNode(500, 200);
        _blazorDiagram.Nodes.Add(new[] { node1, node2, node3 });
        _blazorDiagram.Links.Add(new LinkModel(node1.GetPort(PortAlignment.Right), node2.GetPort(PortAlignment.Left)));
    }

    private void AddEmptyGroup()
    {
        _blazorDiagram.Groups.Add(new GroupModel(Array.Empty<NodeModel>())
        {
            Position = new Point(100, 100)
        });
    }

    private void AddChildToGroup()
    {
        if (_blazorDiagram.Groups.Count == 0)
            return;

        foreach (var node in _blazorDiagram.Nodes)
        {
            if (node.Group == null)
            {
                _blazorDiagram.Groups[0].AddChild(node);
                _blazorDiagram.Refresh();
                return;
            }
        }
    }

    private void RemoveChildFromGroup()
    {
        foreach (var node in _blazorDiagram.Nodes)
        {
            if (node.Group != null)
            {
                node.Group.RemoveChild(node);
                _blazorDiagram.Refresh();
                return;
            }
        }
    }

    private NodeModel NewNode(double x, double y)
    {
        var node = new NodeModel(new Point(x, y));
        node.AddPort(PortAlignment.Bottom);
        node.AddPort(PortAlignment.Top);
        node.AddPort(PortAlignment.Left);
        node.AddPort(PortAlignment.Right);
        return node;
    }
}
