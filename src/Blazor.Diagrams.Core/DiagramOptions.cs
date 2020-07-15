using Blazor.Diagrams.Core.Models;
using System;

namespace Blazor.Diagrams.Core
{
    public class DiagramOptions
    {
        public string DeleteKey { get; set; } = "Delete";
        public bool InverseZoom { get; set; }
        public LinkType DefaultLinkType { get; set; }
        public Type? DefaultLinkComponent { get; set; }
        public Type? DefaultNodeComponent { get; set; }
        public int? GridSize { get; set; }
    }
}
