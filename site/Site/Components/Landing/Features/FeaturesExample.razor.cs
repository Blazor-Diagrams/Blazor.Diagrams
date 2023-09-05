using Blazor.Diagrams;
using Blazor.Diagrams.Core.Anchors;
using Blazor.Diagrams.Core.Controls.Default;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.PathGenerators;
using Blazor.Diagrams.Core.Positions;
using Blazor.Diagrams.Core.Routers;
using Site.Models.Landing;

namespace Site.Components.Landing.Features;

public partial class FeaturesExample
{
    private readonly BlazorDiagram _diagram = new();

    protected override void OnInitialized()
    {
        _diagram.Options.Zoom.Enabled = false;
        _diagram.Options.GridSize = 10;
        _diagram.Options.Links.RequireTarget = false;
        _diagram.Options.Links.DefaultPathGenerator = new StraightPathGenerator();
        _diagram.RegisterComponent<ColoredNodeModel, ColoredNodeWidget>();
        _diagram.RegisterComponent<LinkLabelModel, LinkLabelWidget>();

        var smoothPathGenerator = new SmoothPathGenerator();

        // ArrowHeadControl example
        var node1 = _diagram.Nodes.Add(new ColoredNodeModel("Locked", true, "color2", new Point(20, 20)));
        node1.Locked = true;
        var link1 = _diagram.Links.Add(new LinkModel(new ShapeIntersectionAnchor(node1), new PositionAnchor(new Point(340, 60)))
        {
            Color = "#DC9A7A",
            SelectedColor = "#874423",
            TargetMarker = LinkMarker.Arrow
        });
        _diagram.Controls.AddFor(link1).Add(new ArrowHeadControl(false));
        link1.Labels.Add(new LinkLabelModel(link1, "I am a free link", 0.5, new Point(0, -15)));

        // Labels example
        var node2 = _diagram.Nodes.Add(new ColoredNodeModel("Movable", true, "color1", new Point(20, 350)));
        var link2 = _diagram.Links.Add(new LinkModel(node1, node2)
        {
            Color = "#DC9A7A",
            SelectedColor = "#874423",
            TargetMarker = LinkMarker.Arrow
        });
        link2.Labels.Add(new LinkLabelModel(link1, "Start", 0.1, new Point(0, 0)));
        link2.Labels.Add(new LinkLabelModel(link1, "Middle", 0.5, new Point(0, 0)));
        link2.Labels.Add(new LinkLabelModel(link1, "End", 0.9, new Point(0, 0)));

        // Controls example
        var node3 = _diagram.Nodes.Add(new ColoredNodeModel("Select me", false, "color3", new Point(320, 370)));
        var link3 = _diagram.Links.Add(new LinkModel(node1, node3)
        {
            Color = "#9EA5E3",
            SelectedColor = "#2b3595",
            PathGenerator = smoothPathGenerator,
            TargetMarker = LinkMarker.Arrow
        });
        link3.Labels.Add(new LinkLabelModel(link3, "Select me to show controls", 0.5, new Point(0, -15)));

        _diagram.Controls.AddFor(link3)
            .Add(new ArrowHeadControl(true))
            .Add(new ArrowHeadControl(false))
            .Add(new BoundaryControl())
            .Add(new RemoveControl(new LinkPathPositionProvider(0.8, 0, -10)));

        _diagram.Controls.AddFor(node3)
            .Add(new BoundaryControl())
            .Add(new RemoveControl(new BoundsBasedPositionProvider(1, 0, 10, -10)));

        // Ports and Routes example
        var node4 = _diagram.Nodes.Add(new ColoredNodeModel("With ports", false, "color1", new Point(560, 20)));
        var node5 = _diagram.Nodes.Add(new ColoredNodeModel("Locked", true, "color2", new Point(720, 350)));
        node5.Locked = true;

        var port1 = node3.AddPort(PortAlignment.Right);
        var port2 = node4.AddPort(PortAlignment.Left);
        var port3 = node4.AddPort(PortAlignment.Right);
        var port4 = node5.AddPort(PortAlignment.Left);
        var port5 = node5.AddPort(PortAlignment.Right);

        var link4 = _diagram.Links.Add(new LinkModel(port1, port2)
        {
            Color = "#9EA5E3",
            SelectedColor = "#2b3595",
            PathGenerator = smoothPathGenerator,
            TargetMarker = LinkMarker.Arrow
        });
        link4.Labels.Add(new LinkLabelModel(link4, "Smooth PathGenerator", 0.5));

        var link5 = _diagram.Links.Add(new LinkModel(port3, port4)
        {
            Color = "#A0B15B",
            SelectedColor = "#515a2b",
            Router = new OrthogonalRouter(),
            PathGenerator = new StraightPathGenerator(20),
            TargetMarker = new LinkMarker("M 0 -8 L 8 -8 4 0 8 8 0 8 4 0 z", 8)
        });
        link5.Labels.Add(new LinkLabelModel(link5, "Orthogonal Router", 0.3));
        link5.Labels.Add(new LinkLabelModel(link5, "Custom marker", 0.8));

        _diagram.Links.Add(new LinkModel(port3, port5)
        {
            Color = "#A0B15B",
            SelectedColor = "#515a2b",
            Router = new OrthogonalRouter(),
            PathGenerator = new StraightPathGenerator(),
            TargetMarker = LinkMarker.Arrow
        });
    }
}
