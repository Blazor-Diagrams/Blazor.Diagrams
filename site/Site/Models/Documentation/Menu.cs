namespace Site.Models.Documentation;

public record Menu(IEnumerable<MenuItem> Items, IEnumerable<MenuGroup> Groups);

public record MenuGroup(string Title, IEnumerable<MenuItem> Children, string? Icon = null);

public record MenuItem(string Title, string Link, string? Icon = null);
