using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models.Base;
using System;

namespace Blazor.Diagrams.Core.Models;

public class ResizerModel : Model
{
    public ResizerModel(NodeModel parent, ResizerPosition alignment)
    {
        Parent = parent;
        Alignment = alignment;
    }

    public NodeModel Parent { get; }
    public ResizerPosition Alignment { get; }
}