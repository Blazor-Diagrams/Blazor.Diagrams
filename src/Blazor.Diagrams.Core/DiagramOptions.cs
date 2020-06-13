using Blazor.Diagrams.Core.Models;

namespace Blazor.Diagrams.Core
{
    public class DiagramOptions
    {
        public string DeleteKey { get; set; } = "Delete";
        public bool InverseZoom { get; set; }
        public LinkType DefaultLinkType { get; set; }
    }
}
