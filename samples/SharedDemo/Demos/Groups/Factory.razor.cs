using Blazor.Diagrams;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;

namespace SharedDemo.Demos.Groups;

public partial class Factory
{
    protected readonly BlazorDiagram BlazorDiagram = new BlazorDiagram();

    protected override void OnInitialized()
    {
        base.OnInitialized();

        LayoutData.Title = "Groups Factory";
        LayoutData.Info = "Factory setting is a way to customize how groups (models) are created when the user uses the shortcut. " +
            "Try to group nodes using CTRL+ALT+G now.";
        LayoutData.DataChanged();

        BlazorDiagram.Options.Groups.Enabled = true;
        BlazorDiagram.Options.LinksLayerOrder = 2;
        BlazorDiagram.Options.NodesLayerOrder = 1;
        BlazorDiagram.Options.Groups.Factory = (diagram, children) =>
        {
            var group = new GroupModel(children, 25);
            group.AddPort(PortAlignment.Top);
            group.AddPort(PortAlignment.Bottom);
            group.AddPort(PortAlignment.Right);
            group.AddPort(PortAlignment.Left);
            return group;
        };

        var node1 = NewNode(50, 50);
        var node2 = NewNode(250, 250);
        var node3 = NewNode(500, 100);
        BlazorDiagram.Nodes.Add(new[] { node1, node2, node3 });

        BlazorDiagram.Links.Add(new LinkModel(node1.GetPort(PortAlignment.Right), node2.GetPort(PortAlignment.Left)));
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
