using Blazor.Diagrams;
using Blazor.Diagrams.Core.Behaviors;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;

namespace SharedDemo.Demos.Groups;

public partial class CustomShortcut
{
    private BlazorDiagram _blazorDiagram = new BlazorDiagram();

    protected override void OnInitialized()
    {
        base.OnInitialized();

        LayoutData.Title = "Custom Shortcut";
        LayoutData.Info = "You can customize what needs to be pressed to group selected nodes. Ctrl+Shift+k in this example.";
        LayoutData.DataChanged();

        _blazorDiagram.Options.Groups.Enabled = true;
        _blazorDiagram.Options.LinksLayerOrder = 2;
        _blazorDiagram.Options.NodesLayerOrder = 1;
        var ksb = _blazorDiagram.GetBehavior<KeyboardShortcutsBehavior>();
        ksb.RemoveShortcut("g", true, false, true);
        ksb.SetShortcut("k", true, true, false, KeyboardShortcutsDefaults.Grouping);

        var node1 = NewNode(50, 50);
        var node2 = NewNode(250, 250);
        var node3 = NewNode(500, 100);
        _blazorDiagram.Nodes.Add(new[] { node1, node2, node3 });

        _blazorDiagram.Links.Add(new LinkModel(node1.GetPort(PortAlignment.Right), node2.GetPort(PortAlignment.Left)));
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
