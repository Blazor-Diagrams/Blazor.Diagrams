using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;

namespace Blazor.Diagrams.Models
{
    public class SvgNodeModel : NodeModel
    {
        public SvgNodeModel(Point? position = null, ShapeDefiner? shape = null) : base(position, shape)
        {
        }

        public SvgNodeModel(string id, Point? position = null, ShapeDefiner? shape = null) : base(id, position, shape)
        {
        }
    }
}
