using Blazor.Diagrams.Core.Options;

namespace Blazor.Diagrams.Options;

public class BlazorDiagramOptions : DiagramOptions
{
    public int LinksLayerOrder { get; set; } = 0;
    public int NodesLayerOrder { get; set; } = 0;
    
    public new BlazorDiagramZoomOptions Zoom { get; set; } = new();
    public new BlazorDiagramLinkOptions Links { get; set; } = new();
    public new BlazorDiagramGroupOptions Groups { get; set; } = new();
    public new BlazorDiagramConstraintsOptions Constraints { get; set; } = new();
}