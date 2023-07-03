using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Site.Pages.Documentation;

public class DocumentationPage : ComponentBase
{
    [Inject] protected IJSRuntime JSRuntime { get; set; } = null!;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await JSRuntime.InvokeVoidAsync("Prism.highlightAll");
    }
}
