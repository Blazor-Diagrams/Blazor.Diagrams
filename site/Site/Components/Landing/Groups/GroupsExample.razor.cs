using Blazor.Diagrams;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.PathGenerators;
using Site.Models.Landing;
using Site.Models.Landing.Groups;

namespace Site.Components.Landing.Groups;

public partial class GroupsExample
{
    private readonly BlazorDiagram _diagram = new();

    protected override void OnInitialized()
    {
        _diagram.Options.Zoom.Enabled = false;
        _diagram.Options.GridSize = 30;
        _diagram.Options.LinksLayerOrder = 2;
        _diagram.Options.NodesLayerOrder = 1;
        _diagram.Options.Links.DefaultPathGenerator = new StraightPathGenerator();
        _diagram.RegisterComponent<ColoredGroupModel, GroupWidget>();
        _diagram.RegisterComponent<ColoredNodeModel, ColoredNodeWidget>();

        var node1 = _diagram.Nodes.Add(new ColoredNodeModel("Node 1", false, "color3", new Point(90, 60)));
        var node2 = _diagram.Nodes.Add(new ColoredNodeModel("Node 2", false, "color3", new Point(390, 60)));
        var node3 = _diagram.Nodes.Add(new ColoredNodeModel("Node 3", false, "color1", new Point(180, 240)));
        var node4 = _diagram.Nodes.Add(new ColoredNodeModel("Node 4", false, "color1", new Point(330, 240)));

        var group1 = _diagram.Groups.Add(new ColoredGroupModel(new[] { node3, node4 }, "color2"));
        var group2 = _diagram.Groups.Add(new ColoredGroupModel(new[] { (NodeModel)group1, node1, node2 }, "color2"));

        _diagram.Links.Add(new LinkModel(node1, node2)
        {
            TargetMarker = LinkMarker.Arrow,
        });
        _diagram.Links.Add(new LinkModel(node2, node3)
        {
            TargetMarker = LinkMarker.Arrow,
        });
        _diagram.Links.Add(new LinkModel(node3, node1)
        {
            TargetMarker = LinkMarker.Arrow,
        });
        _diagram.Links.Add(new LinkModel(node2, group1)
        {
            TargetMarker = LinkMarker.Arrow,
        });
    }
}
