using System;
using Blazor.Diagrams.Core.Models;

namespace Blazor.Diagrams.Core.Options;

public class DiagramLinkOptions
{
    private double _snappingRadius = 50;
    
    public Router DefaultRouter { get; set; } = Routers.Normal;
    public PathGenerator DefaultPathGenerator { get; set; } = PathGenerators.Smooth;
    public bool EnableSnapping { get; set; } = false;
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
    public LinkFactory Factory { get; set; } = (diagram, sourcePort) => new LinkModel(sourcePort);
}