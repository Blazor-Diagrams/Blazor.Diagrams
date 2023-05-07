using Site.Models.Documentation;

namespace Site.Static
{
    public static class Documentation
    {
        public static readonly Menu Menu = new(new List<MenuItem>
        {
            new MenuItem("Documentation", "/documentation", Icons.BookOpen),
            new MenuItem("Examples", "/examples", Icons.FolderOpen),
        }, new List<MenuGroup>
        {
            new MenuGroup("Getting Started", new List<MenuItem>
            {
                new MenuItem("Installation", "/documentation/installation"),
                new MenuItem("Diagram Creation", "/documentation/diagram-creation"),
                new MenuItem("Display", "/documentation/display"),
            }),
            new MenuGroup("Diagram", new List<MenuItem>
            {
                new MenuItem("Layers", "/documentation/diagram-layers"),
                new MenuItem("Behaviors", "/documentation/diagram-behaviors"),
                new MenuItem("Options", "/documentation/diagram-options"),
                new MenuItem("Keyboard Shortcuts", "/documentation/keyboard-shortcuts"),
                new MenuItem("API", "/documentation/diagram-api"),
            }),
            new MenuGroup("Groups", new List<MenuItem>
            {
                new MenuItem("Overview", "/documentation/groups-overview"),
                new MenuItem("SVG", "/documentation/groups-svg"),
            }),
            new MenuGroup("Customization", new List<MenuItem>
            {
                new MenuItem("Custom Nodes", "/documentation/custom-nodes"),
                new MenuItem("Custom Ports", "/documentation/custom-ports"),
                new MenuItem("Custom Links", "/documentation/custom-links"),
                new MenuItem("Custom Groups", "/documentation/custom-groups"),
            })
        });
    }
}
