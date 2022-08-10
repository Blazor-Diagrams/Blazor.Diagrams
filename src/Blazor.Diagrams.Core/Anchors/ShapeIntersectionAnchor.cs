using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;
using System.Collections.Generic;

namespace Blazor.Diagrams.Core.Anchors
{
    public class ShapeIntersectionAnchor : Anchor
    {
        public ShapeIntersectionAnchor(NodeModel node, Point? offset = null) : base(node, offset) { }

        public override Point? GetPosition(BaseLinkModel link, Point[] route)
        {
            if (Node.Size == null)
                return null;

            var isTarget = link.Target == this;
            var nodeCenter = Node.GetBounds()!.Center;
            Point? pt;
            if (route.Length > 0)
            {
                pt = route[isTarget ? ^1 : 0];
            }
            else
            {
                pt = GetOtherPosition(link, isTarget);
            }

            if (pt is null) return null;

            var line = new Line(pt, nodeCenter);
            var intersections = Node.GetShape().GetIntersectionsWithLine(line);
            return GetClosestPointTo(intersections, pt);
        }

        private static Point? GetOtherPosition(BaseLinkModel link, bool isTarget)
        {
            if (!isTarget && link.Target == null)
                return link.OnGoingPosition;

            var anchor = isTarget ? link.Source : link.Target!;
            return anchor switch
            {
                SinglePortAnchor spa => spa.Port.MiddlePosition,
                ShapeIntersectionAnchor sia => sia.Node.GetBounds()?.Center ?? null,
                _ => throw new DiagramsException($"Unhandled Anchor type {anchor.GetType().Name} when trying to find intersection")
            };
        }

        private static Point? GetClosestPointTo(IEnumerable<Point> points, Point point)
        {
            var minDist = double.MaxValue;
            Point? minPoint = null;

            foreach (var pt in points)
            {
                var dist = pt.DistanceTo(point);
                if (dist < minDist)
                {
                    minDist = dist;
                    minPoint = pt;
                }
            }

            return minPoint;
        }
    }
}
