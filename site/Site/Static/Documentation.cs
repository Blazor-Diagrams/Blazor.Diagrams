using Site.Models.Documentation;

namespace Site.Static;

public static class Documentation
{
    public static readonly Menu Menu = new(new List<MenuItem>
    {
        new MenuItem("Documentation", "/documentation", Icons.BookOpen),
        //new MenuItem("Examples", "/examples", Icons.FolderOpen),
    }, new List<MenuGroup>
    {
        new MenuGroup("Getting Started", new List<MenuItem>
        {
            new MenuItem("Installation", "/documentation/installation"),
            new MenuItem("Diagram Creation", "/documentation/diagram-creation"),
            new MenuItem("Display", "/documentation/display"),
        }, Icon: Icons.Awards),
        new MenuGroup("Diagram", new List<MenuItem>
        {
            new MenuItem("Overview", "/documentation/diagram"),
            new MenuItem("Behaviors", "/documentation/diagram-behaviors"),
            new MenuItem("Ordering", "/documentation/diagram-ordering"),
            new MenuItem("Options", "/documentation/diagram-options"),
            new MenuItem("Keyboard Shortcuts", "/documentation/keyboard-shortcuts"),
            new MenuItem("API", "/documentation/diagram-api"),
        }, Icon: Icons.ListTree),
        new MenuGroup("Nodes", new List<MenuItem>
        {
            new MenuItem("Overview", "/documentation/nodes"),
            new MenuItem("SVG", "/documentation/nodes-svg"),
            new MenuItem("Customization", "/documentation/nodes-customization"),
            new MenuItem("Customization (SVG)", "/documentation/nodes-customization-svg")
        }, Icon: Icons.Square),
        new MenuGroup("Ports", new List<MenuItem>
        {
            new MenuItem("Overview", "/documentation/ports"),
            new MenuItem("Customization", "/documentation/ports-customization")
        }, Icon: Icons.Circle),
        new MenuGroup("Links", new List<MenuItem>
        {
            new MenuItem("Overview", "/documentation/links"),
            new MenuItem("Anchors", "/documentation/links-anchors"),
            new MenuItem("Routers", "/documentation/links-routers"),
            new MenuItem("Path Generators", "/documentation/links-path-generators"),
            new MenuItem("Markers", "/documentation/links-markers"),
            new MenuItem("Vertices", "/documentation/links-vertices"),
        }, Icon: Icons.Link),
        new MenuGroup("Groups", new List<MenuItem>
        {
            new MenuItem("Overview", "/documentation/groups"),
            new MenuItem("SVG", "/documentation/groups-svg"),
            new MenuItem("Customization", "/documentation/groups-customization"),
            new MenuItem("Customization (SVG)", "/documentation/groups-customization-svg")
        }, Icon: Icons.Ratio),
        new MenuGroup("Diagram Widgets", new List<MenuItem>
        {
            new MenuItem("Navigator", "/documentation/navigator-widget"),
            new MenuItem("Grid", "/documentation/grid-widget"),
            new MenuItem("Selection Box", "/documentation/selection-box-widget"),
        }, Icon: Icons.Components),
        new MenuGroup("Controls", new List<MenuItem>
        {
            new MenuItem("Overview", "/documentation/controls"),
            new MenuItem("Customization", "/documentation/controls-customization"),
        }, Icon: Icons.Controls),
        new MenuGroup("Misc", new List<MenuItem>
        {
            new MenuItem("Position Providers", "/documentation/position-providers")
        })
    });
}
