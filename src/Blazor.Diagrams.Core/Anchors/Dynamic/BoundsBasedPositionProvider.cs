using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;

namespace Blazor.Diagrams.Core.Anchors.Dynamic;

public class BoundsBasedPositionProvider : IDynamicAnchorPositionProvider
{
    public BoundsBasedPositionProvider(double x, double y, double offsetX = 0, double offsetY = 0)
    {
        X = x;
        Y = y;
        OffsetX = offsetX;
        OffsetY = offsetY;
    }

    public double X { get; }
    public double Y { get; }
    public double OffsetX { get; }
    public double OffsetY { get; }

    public Point GetPosition(NodeModel node, BaseLinkModel _)
    {
        var bounds = node.GetBounds()!;
        return new Point(bounds.Left + X * bounds.Width + OffsetX, bounds.Top + Y * bounds.Height + OffsetY);
    }
}