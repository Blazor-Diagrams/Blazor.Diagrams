using Blazor.Diagrams.Core.Models;

namespace Blazor.Diagrams.Core.Options;

public class DiagramGroupOptions
{
    public bool Enabled { get; set; }
    
    public GroupFactory Factory { get; set; } = (diagram, children) => new GroupModel(children);
}