using System;
using Blazor.Diagrams.Core.Anchors;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.PathGenerators;
using Blazor.Diagrams.Core.Routers;

namespace Blazor.Diagrams.Core.Options;

public class DiagramLinkOptions
{
    private double _snappingRadius = 50;

    public Router DefaultRouter { get; set; } = new NormalRouter();
    public PathGenerator DefaultPathGenerator { get; set; } = new SmoothPathGenerator();
    public bool EnableSnapping { get; set; } = false;
    public bool RequireTarget { get; set; } = true;

    public double SnappingRadius
    {
        get => _snappingRadius;
        set
        {
            if (value <= 0)
                throw new ArgumentException($"SnappingRadius must be greater than zero");

            _snappingRadius = value;
        }
    }

    public LinkFactory Factory { get; set; } = (diagram, source, targetAnchor) =>
    {
        Anchor sourceAnchor = source switch
        {
            NodeModel node => new ShapeIntersectionAnchor(node),
            PortModel port => new SinglePortAnchor(port),
            _ => throw new NotImplementedException()
        };
        return new LinkModel(sourceAnchor, targetAnchor);
    };

    public AnchorFactory TargetAnchorFactory { get; set; } = (diagram, link, model) =>
    {
        return model switch
        {
            NodeModel node => new ShapeIntersectionAnchor(node),
            PortModel port => new SinglePortAnchor(port),
            _ => throw new ArgumentOutOfRangeException(nameof(model), model, null)
        };
    };
}