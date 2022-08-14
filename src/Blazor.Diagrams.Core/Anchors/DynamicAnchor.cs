using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;
using System.Collections.Generic;
using System.Linq;

namespace Blazor.Diagrams.Core.Anchors
{
    // Figure out a better name
    // Generic?
    public class DynamicAnchor : Anchor
    {
        public DynamicAnchor(NodeModel node, DynamicAnchorPosition[] positions, Point? offset = null) : base(node, offset)
        {
            Positions = positions;
        }

        public DynamicAnchorPosition[] Positions { get; }

        public override Point? GetPosition(BaseLinkModel link, Point[] route)
        {
            if (Node.Size == null)
                return null;

            var isTarget = link.Target == this;
            var pt = route.Length > 0 ? route[isTarget ? ^1 : 0] : GetOtherPosition(link, isTarget);
            return pt is null ? null : GetClosestPointTo(CalculatePositions(), pt);
        }

        private IEnumerable<Point> CalculatePositions()
        {
            var bounds = Node.GetBounds()!;
            return Positions.Select(e =>
            {
                return new Point(
                    bounds.Left + e.Position.X * bounds.Width + (e.Offset?.X ?? 0),
                    bounds.Top + e.Position.Y * bounds.Height + (e.Offset?.Y ?? 0)
                );
            });
        }
    }

    public record DynamicAnchorPosition
    {
        public DynamicAnchorPosition(Point position, Point offset)
        {
            Position = position;
            Offset = offset;
        }

        public DynamicAnchorPosition(double x, double y) : this(new Point(x, y), Point.Zero) { }

        public DynamicAnchorPosition(double x, double y, double offsetX, double offsetY) : this(new Point(x, y), new Point(offsetX, offsetY)) { }

        public Point Position { get; }
        public Point Offset { get; }
    }
}
