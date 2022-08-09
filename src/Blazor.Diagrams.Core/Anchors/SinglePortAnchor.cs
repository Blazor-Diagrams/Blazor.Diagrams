using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;

namespace Blazor.Diagrams.Core.Anchors
{
    public class SinglePortAnchor : Anchor
    {
        public SinglePortAnchor(PortModel port, Point? offset = null) : base(port.Parent, offset)
        {
            Port = port;
        }

        public PortModel Port { get; }

        public override Point? GetPosition(BaseLinkModel link, Point[] route)
        {
            if (!Port.Initialized)
                return null;

            if ((link.Source == this && link.SourceMarker is null) || (link.Target == this && link.TargetMarker is null))
                return Port.MiddlePosition;

            var pt = Port.Position;
            switch (Port.Alignment)
            {
                case PortAlignment.Top:
                    return new Point(pt.X + Port.Size.Width / 2, pt.Y);
                case PortAlignment.TopRight:
                    return new Point(pt.X + Port.Size.Width, pt.Y);
                case PortAlignment.Right:
                    return new Point(pt.X + Port.Size.Width, pt.Y + Port.Size.Height / 2);
                case PortAlignment.BottomRight:
                    return new Point(pt.X + Port.Size.Width, pt.Y + Port.Size.Height);
                case PortAlignment.Bottom:
                    return new Point(pt.X + Port.Size.Width / 2, pt.Y + Port.Size.Height);
                case PortAlignment.BottomLeft:
                    return new Point(pt.X, pt.Y + Port.Size.Height);
                case PortAlignment.Left:
                    return new Point(pt.X, pt.Y + Port.Size.Height / 2);
                default:
                    return pt;
            }
        }
    }
}
