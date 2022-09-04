using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;

namespace Blazor.Diagrams.Models;

public class SvgNodeModel : NodeModel
{
    public SvgNodeModel(Point? position = null) : base(position)
    {
    }

    public SvgNodeModel(string id, Point? position = null) : base(id, position)
    {
    }
}