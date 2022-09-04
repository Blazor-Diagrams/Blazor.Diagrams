namespace Blazor.Diagrams.Core.Options;

public class DiagramOptions
{
    public int? GridSize { get; set; }
    public bool AllowMultiSelection { get; set; } = true;
    public bool AllowPanning { get; set; } = true;
    public bool EnableVirtualization { get; set; } = true; // Todo: behavior
    
    public virtual DiagramZoomOptions Zoom { get; } = new();
    public virtual DiagramLinkOptions Links { get; } = new();
    public virtual DiagramGroupOptions Groups { get; } = new();
    public virtual DiagramConstraintsOptions Constraints { get; } = new();
}