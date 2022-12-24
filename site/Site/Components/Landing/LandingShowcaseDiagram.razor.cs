using Blazor.Diagrams;
using Blazor.Diagrams.Core.Anchors;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;
using Site.Models.Landing;

namespace Site.Components.Landing;

public partial class LandingShowcaseDiagram
{
    private readonly BlazorDiagram _diagram = new();

    protected override void OnInitialized()
    {
        _diagram.Options.Zoom.Enabled = false;
        _diagram.Options.GridSize = 20;
        _diagram.RegisterComponent<AddNodeModel, AddNodeWidget>();
        _diagram.RegisterComponent<NumberNodeModel, NumberNodeWidget>();

        _diagram.Nodes.Added += OnNodeAdded;
        _diagram.Nodes.Removed += OnNodeRemoved;
        _diagram.Links.Added += OnLinkAdded;
        _diagram.Links.Removed += OnLinkRemoved;

        var n1 = _diagram.Nodes.Add(new NumberNodeModel(new Point(200, 100)));
        var n2 = _diagram.Nodes.Add(new NumberNodeModel(new Point(200, 260)));
        var n3 = _diagram.Nodes.Add(new NumberNodeModel(new Point(480, 260)));
        var a1 = _diagram.Nodes.Add(new AddNodeModel(new Point(500, 100)));
        var a2 = _diagram.Nodes.Add(new AddNodeModel(new Point(750, 200)));

        _diagram.Links.Add(new LinkModel(n1.Ports[0], a1.Ports[0]));
        _diagram.Links.Add(new LinkModel(n2.Ports[0], a1.Ports[1]));
        _diagram.Links.Add(new LinkModel(a1.Ports[2], a2.Ports[0]));
        _diagram.Links.Add(new LinkModel(n3.Ports[0], a2.Ports[1]));

        n1.Value = 3;
        n2.Value = 91;
        n3.Value = 25;
    }

    private void OnNodeAdded(NodeModel node)
    {
        (node as BaseOperation)!.ValueChanged += OnValueChanged;
    }

    private void OnNodeRemoved(NodeModel node)
    {
        (node as BaseOperation)!.ValueChanged -= OnValueChanged;
    }

    private void OnValueChanged(BaseOperation op)
    {
        foreach (var link in _diagram.Links)
        {
            var sp = (link.Source as SinglePortAnchor)!;
            var tp = (link.Target as SinglePortAnchor)!;
            var otherNode = sp.Port.Parent == op ? tp.Port.Parent : sp.Port.Parent;
            otherNode.Refresh();
        }
    }

    private void OnLinkAdded(BaseLinkModel link)
    {
        link.TargetChanged += OnLinKTargetChanged;
    }

    private void OnLinKTargetChanged(BaseLinkModel link, Anchor? oldTarget, Anchor? newTarget)
    {
        if (oldTarget == null && newTarget != null) // First attach
        {
            (newTarget.Model as PortModel)!.Parent.Refresh();
        }
    }

    private void OnLinkRemoved(BaseLinkModel link)
    {
        (link.Source.Model as PortModel)!.Parent.Refresh();
        if (link.Target != null) (link.Target.Model as PortModel)!.Parent.Refresh();
        link.TargetChanged -= OnLinKTargetChanged;
    }
}