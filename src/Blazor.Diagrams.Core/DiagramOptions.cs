using Blazor.Diagrams.Core.Models;
using System;

namespace Blazor.Diagrams.Core
{
    public class DiagramOptions
    {
        public string DeleteKey { get; set; } = "Delete";
        public bool InverseZoom { get; set; }
        public Type? DefaultNodeComponent { get; set; }
        public int? GridSize { get; set; }
        public bool GroupingEnabled { get; set; }
        public bool AllowMultiSelection { get; set; } = true;
        public bool AllowPanning { get; set; } = true;
        public bool AllowZooming { get; set; } = true;
        public DiagramLinkOptions Links { get; set; } = new DiagramLinkOptions();
    }

    public class DiagramLinkOptions
    {
        public LinkType DefaultLinkType { get; set; }
        public Type? DefaultLinkComponent { get; set; }
        public string DefaultColor { get; set; } = "black";
        public string DefaultSelectedColor { get; set; } = "rgb(110, 159, 212)";
    }
}
