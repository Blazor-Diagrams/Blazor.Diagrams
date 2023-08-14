using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;

namespace Site.Models.Landing;

public class ColoredNodeModel : NodeModel
{
    public ColoredNodeModel(string title, bool round, string color, Point position) : base(position)
    {
        Title = title;
        Round = round;
        Color = color;
    }

    public bool Round { get; }
    public string Color { get; }

    public override IShape GetShape()
    {
        return Round ? Shapes.Circle(this) : Shapes.Rectangle(this);
    }
}
