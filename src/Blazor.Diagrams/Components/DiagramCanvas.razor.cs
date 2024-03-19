﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace Blazor.Diagrams.Components;

public partial class DiagramCanvas : IAsyncDisposable
{
    private DotNetObjectReference<DiagramCanvas>? _reference;
    private bool _shouldRender;

    protected ElementReference elementReference;

    [CascadingParameter] public BlazorDiagram BlazorDiagram { get; set; } = null!;

    [Parameter] public RenderFragment? Widgets { get; set; }

    [Parameter] public RenderFragment? AdditionalSvg { get; set; }

    [Parameter] public RenderFragment? AdditionalHtml { get; set; }

    [Parameter] public string? Class { get; set; }

    [Inject] public IJSRuntime JSRuntime { get; set; } = null!;

    private IJSObjectReference? module;

    public async ValueTask DisposeAsync()
    {
        BlazorDiagram.Changed -= OnDiagramChanged;

        if (_reference == null)
        {
            return;
        }

        if (elementReference.Id != null)
            await JSRuntime.UnobserveResizes(elementReference);
        if(module is not null)
        {
            await module.DisposeAsync();
        }

        _reference.Dispose();
    }

    private string GetLayerStyle(int order)
    {
        return FormattableString.Invariant(
            $"transform: translate({BlazorDiagram.Pan.X}px, {BlazorDiagram.Pan.Y}px) scale({BlazorDiagram.Zoom}); z-index: {order};");
    }

    protected override void OnInitialized()
    {
        base.OnInitialized();

        _reference = DotNetObjectReference.Create(this);
        BlazorDiagram.Changed += OnDiagramChanged;
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender)
        {
            module ??= await JSRuntime.InvokeAsync<IJSObjectReference>("import","./_content/Z.Blazor.Diagrams/Components/DiagramCanvas.razor.js");
            BlazorDiagram.SetContainer(await module.GetBoundingClientRect(elementReference));

            await module.ObserveResizes(elementReference, _reference!);
        }
    }

    [JSInvokable]
    public void OnResize(Rectangle rect)
    {
        BlazorDiagram.SetContainer(rect);
    }

    protected override bool ShouldRender()
    {
        if (!_shouldRender) return false;

        _shouldRender = false;
        return true;
    }

    private void OnPointerDown(PointerEventArgs e)
    {
        BlazorDiagram.TriggerPointerDown(null, e.ToCore());
    }

    private void OnPointerMove(PointerEventArgs e)
    {
        BlazorDiagram.TriggerPointerMove(null, e.ToCore());
    }

    private void OnPointerUp(PointerEventArgs e)
    {
        BlazorDiagram.TriggerPointerUp(null, e.ToCore());
    }

    private void OnKeyDown(KeyboardEventArgs e)
    {
        BlazorDiagram.TriggerKeyDown(e.ToCore());
    }

    private void OnWheel(WheelEventArgs e)
    {
        BlazorDiagram.TriggerWheel(e.ToCore());
    }

    private void OnDiagramChanged()
    {
        _shouldRender = true;
        InvokeAsync(StateHasChanged);
    }
}