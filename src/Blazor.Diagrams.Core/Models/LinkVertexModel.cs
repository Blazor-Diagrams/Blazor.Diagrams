﻿using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models.Base;

namespace Blazor.Diagrams.Core.Models;

public class LinkVertexModel : MovableModel
{
    public LinkVertexModel(BaseLinkModel parent, Point? position = null) : base(position)
    {
        Parent = parent;
    }

    public BaseLinkModel Parent { get; }

    public override void SetPosition(double x, double y)
    {
        base.SetPosition(x, y);
        Refresh();
        Parent.Refresh();
    }
}
