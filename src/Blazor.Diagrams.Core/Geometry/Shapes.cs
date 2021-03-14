using Blazor.Diagrams.Core.Models;

namespace Blazor.Diagrams.Core.Geometry
{
    public delegate IShape ShapeDefiner(NodeModel node);

    public static class Shapes
    {
        public static IShape Rectangle(NodeModel node) => new Rectangle(node.Position, node.Size!);

        public static IShape Circle(NodeModel node)
        {
            var halfWidth = node.Size!.Width / 2;
            var centerX = node.Position.X + halfWidth;
            var centerY = node.Position.Y + node.Size!.Height / 2;
            return new Ellipse(centerX, centerY, halfWidth, halfWidth);
        }
    }
}
