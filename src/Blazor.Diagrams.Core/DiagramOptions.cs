using Blazor.Diagrams.Core.Models;
using System;
using System.ComponentModel;

namespace Blazor.Diagrams.Core
{
    public class DiagramOptions
    {
        [Description("Key code for deleting entities")]
        public string DeleteKey { get; set; } = "Delete";
        [Description("Whether to inverse the zoom direction or not")]
        public bool InverseZoom { get; set; }
        [Description("The default component for nodes")]
        public Type? DefaultNodeComponent { get; set; }
        [Description("The grid size (grid-based snaping")]
        public int? GridSize { get; set; }
        [Description("Whether to enable the ability to group nodes together using [CTRL+ALT+G] or not")]
        public bool GroupingEnabled { get; set; }
        [Description("Whether to allow users to select multiple nodes at once using CTRL or not")]
        public bool AllowMultiSelection { get; set; } = true;
        [Description("Whether to allow panning or not")]
        public bool AllowPanning { get; set; } = true;
        [Description("Whether to allow zooming or not")]
        public bool AllowZooming { get; set; } = true;

        public DiagramLinkOptions Links { get; set; } = new DiagramLinkOptions();
    }

    public class DiagramLinkOptions
    {
        [Description("The default type of newly created links")]
        public LinkType DefaultLinkType { get; set; }
        [Description("The default component for links")]
        public Type? DefaultLinkComponent { get; set; }
        [Description("The default color for links")]
        public string DefaultColor { get; set; } = "black";
        [Description("The default color for selected links")]
        public string DefaultSelectedColor { get; set; } = "rgb(110, 159, 212)";
    }
}
