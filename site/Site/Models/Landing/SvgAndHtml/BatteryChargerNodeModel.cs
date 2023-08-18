using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;

namespace Site.Models.Landing.SvgAndHtml;

public class BatteryChargerNodeModel : NodeModel
{
    public BatteryChargerNodeModel(Func<int> getter, Action<int> setter, Point position) : base(position)
    {
        Getter = getter;
        Setter = setter;
    }

    public BatteryNodeModel? Battery { get; private set; }
    public Func<int> Getter { get; }
    public Action<int> Setter { get; }
}
