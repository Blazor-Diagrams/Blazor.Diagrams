using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;

namespace Blazor.Diagrams.Core.Anchors.Dynamic;

public interface IDynamicAnchorPositionProvider
{
    public Point GetPosition(NodeModel node, BaseLinkModel link);
}