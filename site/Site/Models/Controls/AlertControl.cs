using Blazor.Diagrams.Core;
using Blazor.Diagrams.Core.Controls;
using Blazor.Diagrams.Core.Events;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;
using Microsoft.JSInterop;

namespace Site.Models.Controls;

public class AlertControl : ExecutableControl
{
    private readonly IJSRuntime _jSRuntime;

    public AlertControl(IJSRuntime jSRuntime)
    {
        _jSRuntime = jSRuntime;
    }

    public override Point? GetPosition(Model model)
    {
        // Fixed at top-right
        var node = (model as NodeModel)!;
        if (node.Size == null)
            return null;

        return node.Position.Add(node.Size.Width, -15);
    }

    public override async ValueTask OnPointerDown(Diagram diagram, Model model, PointerEventArgs e)
    {
        await _jSRuntime.InvokeVoidAsync("alert", "Some Alert??");
    }
}
