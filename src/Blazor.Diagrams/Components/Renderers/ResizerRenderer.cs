using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using System;
using PointerEventArgs = Microsoft.AspNetCore.Components.Web.PointerEventArgs;

namespace Blazor.Diagrams.Components.Renderers;

public class ResizerRenderer : ComponentBase, IDisposable
{
    [CascadingParameter] public BlazorDiagram BlazorDiagram { get; set; } = null!;
    [Parameter] public ResizerModel Resizer { get; set; } = null!;
    [Parameter] public string ResizerClass { get; set; } = "resizer";

    public void Dispose()
    {
    }

    protected override bool ShouldRender()
    {
        return false;
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenElement(1, "div");
        builder.AddAttribute(2, "class", $"{ResizerClass} {Resizer.Alignment.ToString().ToLower()}");
        builder.AddAttribute(3, "onpointerdown", EventCallback.Factory.Create<PointerEventArgs>(this, OnPointerDown));
        builder.AddEventStopPropagationAttribute(4, "onpointerdown", true);
        builder.CloseElement();
    }

    private void OnPointerDown(PointerEventArgs e)
    {
        BlazorDiagram.TriggerPointerDown(Resizer, e.ToCore());
    }
}
