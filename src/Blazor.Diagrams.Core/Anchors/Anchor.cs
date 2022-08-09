using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;
using System;

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
    }
}
