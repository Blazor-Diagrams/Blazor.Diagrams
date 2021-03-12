using Blazor.Diagrams.Core.Models;

namespace Blazor.Diagrams.Core.Geometry
{
    public delegate IShape ShapeDefiner(NodeModel node);

    public static class Shapes
    {
        public static IShape Rectangle(NodeModel node) => new Rectangle(node.Position, node.Size!);
    }
}
