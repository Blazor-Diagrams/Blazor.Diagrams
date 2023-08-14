using Blazor.Diagrams;
using Blazor.Diagrams.Core.Anchors;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.PathGenerators;
using Site.Models.Landing.SvgAndHtml;

namespace Site.Components.Landing.SvgAndHtml;

public partial class SvgAndHtmlExample
{
    private readonly BlazorDiagram _diagram = new();

    protected override void OnInitialized()
    {
        _diagram.Options.GridSize = 30;
        _diagram.Options.Constraints.ShouldDeleteLink = _ => ValueTask.FromResult(false);
        _diagram.Options.Zoom.Enabled = false;
        _diagram.Options.Links.DefaultPathGenerator = new StraightPathGenerator();
        _diagram.RegisterComponent<BatteryNodeModel, BatteryNodeWidget>();
        _diagram.RegisterComponent<NodeModel, BatteryChargerNodeWidget>();

        var battery = new BatteryNodeModel(new Point(90, 150));
        var port1 = battery.AddPort(PortAlignment.Right);
        var port2 = battery.AddPort(PortAlignment.Right);

        _diagram.Nodes.Add(battery);
        var charger1 = _diagram.Nodes.Add(new BatteryChargerNodeModel(() => battery.FirstCharge, i => battery.FirstCharge = i, new Point(300, 60)));
        var charger2 = _diagram.Nodes.Add(new BatteryChargerNodeModel(() => battery.SecondCharge, i => battery.SecondCharge = i, new Point(300, 180)));

        _diagram.Links.Add(new LinkModel(new SinglePortAnchor(port1), new ShapeIntersectionAnchor(charger1)));
        _diagram.Links.Add(new LinkModel(new SinglePortAnchor(port2), new ShapeIntersectionAnchor(charger2)));
    }
}
