using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;

namespace Blazor.Diagrams.Core.Anchors.Dynamic;

public class ShapeAnglePositionProvider : IDynamicAnchorPositionProvider
{
    public ShapeAnglePositionProvider(double angle, double offsetX = 0, double offsetY = 0)
    {
        Angle = angle;
        OffsetX = offsetX;
        OffsetY = offsetY;
    }

    public double Angle { get; }
    public double OffsetX { get; }
    public double OffsetY { get; }
    
    public Point GetPosition(NodeModel node, BaseLinkModel link)
    {
        var shape = node.GetShape();
        return shape.GetPointAtAngle(Angle)?.Add(OffsetX, OffsetY) ?? Point.Zero;
    }
}