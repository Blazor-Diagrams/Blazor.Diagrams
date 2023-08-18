using Blazor.Diagrams.Core.Geometry;

namespace Site.Models.Landing;

public class NumberNodeModel : BaseOperation
{
    public NumberNodeModel(Point position) : base(position)
    {
        AddPort(new CalculatorPortModel(this, false));
    }
}