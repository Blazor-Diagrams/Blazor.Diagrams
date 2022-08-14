using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;
using System;
using System.Collections.Generic;

namespace Blazor.Diagrams.Core.Anchors
{
    public abstract class Anchor
    {
        public Anchor(NodeModel node, Point? offset = null)
        {
            Node = node;
            Offset = offset ?? Point.Zero;
        }

        public NodeModel Node { get; }
        public Point Offset { get; }

        public abstract Point? GetPosition(BaseLinkModel link, Point[] route);

        public Point? GetPosition(BaseLinkModel link) => GetPosition(link, Array.Empty<Point>());

        protected static Point? GetOtherPosition(BaseLinkModel link, bool isTarget)
        {
            if (!isTarget && link.Target == null)
                return link.OnGoingPosition;

            var anchor = isTarget ? link.Source : link.Target!;
            return anchor switch
            {
                SinglePortAnchor spa => spa.Port.MiddlePosition,
                ShapeIntersectionAnchor sia => sia.Node.GetBounds()?.Center ?? null,
                DynamicAnchor da => da.Node.GetBounds()?.Center ?? null,
                _ => throw new DiagramsException($"Unhandled Anchor type {anchor.GetType().Name} when trying to find intersection")
            };
        }

        protected static Point? GetClosestPointTo(IEnumerable<Point> points, Point point)
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
