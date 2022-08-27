namespace Blazor.Diagrams.Core.Options;

public class DiagramOptions
{
    public int? GridSize { get; set; }
    public bool AllowMultiSelection { get; set; } = true;
    public bool AllowPanning { get; set; } = true;
    public bool EnableVirtualization { get; set; } = true; // Todo: behavior
    
    public DiagramZoomOptions Zoom { get; set; } = new();
    public DiagramLinkOptions Links { get; set; } = new();
    public DiagramGroupOptions Groups { get; set; } = new();
    public DiagramConstraintsOptions Constraints { get; set; } = new();
}