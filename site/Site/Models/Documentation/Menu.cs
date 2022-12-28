namespace Site.Models.Documentation
{
    public record Menu(IEnumerable<MenuItem> Items, IEnumerable<MenuGroup> Groups);

    public record MenuGroup(string Title, IEnumerable<MenuItem> Children);

    public record MenuItem(string Title, string Link, string? Icon = null);
}
