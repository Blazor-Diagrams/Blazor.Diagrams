using Blazor.Diagrams.Core.Models;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.ComponentModel;

namespace Blazor.Diagrams.Core
{
    public class DiagramOptions
    {
        [Description("Key code for deleting entities")]
        public string DeleteKey { get; set; } = "Delete";
        [Description("The default component for nodes")]
        public Type? DefaultNodeComponent { get; set; }
        [Description("The grid size (grid-based snaping")]
        public int? GridSize { get; set; }
        [Description("Whether to allow users to select multiple nodes at once using CTRL or not")]
        public bool AllowMultiSelection { get; set; } = true;
        [Description("Whether to allow panning or not")]
        public bool AllowPanning { get; set; } = true;
        [Description("Only render visible nodes")]
        public bool EnableVirtualization { get; set; } = true;

        public DiagramZoomOptions Zoom { get; set; } = new DiagramZoomOptions();
        public DiagramLinkOptions Links { get; set; } = new DiagramLinkOptions();
        public DiagramGroupOptions Groups { get; set; } = new DiagramGroupOptions();
    }

    public class DiagramLinkOptions
    {
        [Description("The default component for links")]
        public Type? DefaultLinkComponent { get; set; }
        [Description("Default Router for links")]
        public Router DefaultRouter { get; set; } = Routers.Normal;
        [Description("Default PathGenerator for links")]
        public PathGenerator DefaultPathGenerator { get; set; } = PathGenerators.Smooth;
        [Description("Whether to enable link snapping")]
        public bool EnableSnapping { get; set; }
        [Description("Link snapping radius")]
        public double SnappingRadius { get; set; } = 50;
        [Description("Link model factory")]
        public LinkFactory Factory { get; set; } = (diagram, sourcePort) => new LinkModel(sourcePort);
    }

    public class DiagramZoomOptions
    {
        [Description("Whether to allow zooming or not")]
        public bool Enabled { get; set; } = true;
        [Description("Whether to inverse the zoom direction or not")]
        public bool Inverse { get; set; }
        [Description("Minimum value allowed")]
        public double Minimum { get; set; } = 0.1;
        [Description("Maximum value allowed")]
        public double Maximum { get; set; } = 2;
    }

    public class DiagramGroupOptions
    {
        [Description("Whether to allow users to group/ungroup nodes")]
        public bool Enabled { get; set; }
        [Description("Keyboard shortcut (CTRL+ALT+G by default)")]
        public Func<KeyboardEventArgs, bool> KeyboardShortcut { get; set; } = e => e.CtrlKey && e.AltKey && e.Key == "g";
        [Description("Group model factory")]
        public GroupFactory Factory { get; set; } = (diagram, children) => new GroupModel(children);
    }
}
