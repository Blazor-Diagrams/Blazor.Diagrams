using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Models;

namespace Site.Models.Landing.SvgAndHtml;

public class BatteryNodeModel : SvgNodeModel
{
    private int _firstCharge = 5;
    private int _secondCharge = 15;

    public BatteryNodeModel(Point position) : base(position)
    {
    }

    public int FirstCharge
    {
        get => _firstCharge;
        set
        {
            _firstCharge = value;
            Refresh();
        }
    }

    public int SecondCharge
    {
        get => _secondCharge;
        set
        {
            _secondCharge = value;
            Refresh();
        }
    }

    public int Percentage => FirstCharge + SecondCharge;
}
