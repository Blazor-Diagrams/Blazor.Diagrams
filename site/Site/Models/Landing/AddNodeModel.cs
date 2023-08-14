using Blazor.Diagrams.Core.Anchors;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;

namespace Site.Models.Landing;

public class AddNodeModel : BaseOperation
{
    public AddNodeModel(Point position) : base(position)
    {
        AddPort(new CalculatorPortModel(this, true));
        AddPort(new CalculatorPortModel(this, true));
        AddPort(new CalculatorPortModel(this, false));
    }

    public override void Refresh()
    {
        var i1 = Ports[0];
        var i2 = Ports[1];

        if (i1.Links.Count > 0 && i2.Links.Count > 0)
        {
            var l1 = i1.Links[0];
            var l2 = i2.Links[0];
            Value = GetInputValue(i1, l1) + GetInputValue(i2, l2);
        }
        else
        {
            Value = 0;
        }
        
        base.Refresh();
    }

    private static double GetInputValue(PortModel port, BaseLinkModel link)
    {
        var sp = (link.Source as SinglePortAnchor)!;
        var tp = (link.Source as SinglePortAnchor)!;
        var p = sp.Port == port ? tp : sp;
        return (p.Port.Parent as BaseOperation)!.Value;
    }
}