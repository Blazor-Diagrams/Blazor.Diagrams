using Blazor.Diagrams;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.PathGenerators;
using Site.Models.Landing;

namespace Site.Components.Landing;

public partial class WidgetsExample
{
    private readonly BlazorDiagram _diagram = new();
    private bool _gridPoints;

    public bool GridPoints
    {
        get => _gridPoints;
        set
        {
            _gridPoints = value;
            _diagram.Refresh();
        }
    }

    protected override void OnInitialized()
    {
        _diagram.Options.Zoom.Enabled = false;
        _diagram.Options.GridSize = 30;
        _diagram.Options.Links.DefaultPathGenerator = new StraightPathGenerator();
        _diagram.RegisterComponent<ColoredNodeModel, ColoredNodeWidget>();

        var node1 = _diagram.Nodes.Add(new ColoredNodeModel("Node 1", false, "color1", new Point(90, 60)));
        var node2 = _diagram.Nodes.Add(new ColoredNodeModel("Node 2", true, "color2", new Point(450, 60)));
        var node3 = _diagram.Nodes.Add(new ColoredNodeModel("Node 3", false, "color3", new Point(270, 240)));

        _diagram.Links.Add(new LinkModel(node1, node2)
        {
            TargetMarker = LinkMarker.Arrow
        });
        _diagram.Links.Add(new LinkModel(node2, node3)
        {
            TargetMarker = LinkMarker.Arrow
        });
        _diagram.Links.Add(new LinkModel(node3, node1)
        {
            TargetMarker = LinkMarker.Arrow
        });
    }
}
