using Microsoft.AspNetCore.Components;

namespace Site.Shared;

public partial class DocumentationLayout
{
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;

    private (string, string) GetMenuItemExtraClasses(string link)
    {
        if (IsActive(link, false))
            return ("font-semibold text-main", "bg-main text-white");

        return ("font-medium hover:text-slate-900", "bg-gray-100 text-black");
    }

    private string GetGroupMenuItemExtraClasses(string link)
    {
        if (IsActive(link, true))
            return "text-palette-main border-palette-main font-semibold";

        return "text-slate-700 hover:border-slate-400 hover:text-slate-900";
    }

    private bool IsActive(string link, bool fullMatch)
    {
        var relativePath = "/" + NavigationManager.ToBaseRelativePath(NavigationManager.Uri).ToLower();
        return (fullMatch && relativePath == link) || (!fullMatch && relativePath.StartsWith(link));
    }
}
