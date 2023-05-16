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
                new MenuItem("Overview", "/documentation/diagram"),
                new MenuItem("Behaviors", "/documentation/diagram-behaviors"),
                new MenuItem("Options", "/documentation/diagram-options"),
                new MenuItem("Keyboard Shortcuts", "/documentation/keyboard-shortcuts"),
                new MenuItem("API", "/documentation/diagram-api"),
            }),
            new MenuGroup("Groups", new List<MenuItem>
            {
                new MenuItem("Overview", "/documentation/groups-overview"),
                new MenuItem("SVG", "/documentation/groups-svg"),
            })
        });
    }
}
