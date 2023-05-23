using System;
using Blazor.Diagrams.Core.Behaviors;
using Blazor.Diagrams.Core.Events;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models.Base;
using Microsoft.AspNetCore.Components;

namespace Blazor.Diagrams.Components.Widgets;

public partial class SelectionBoxWidget : IDisposable
{
    private Rectangle? _selectionBounds;
    private SelectionBoxBehavior? _selectionBoxBehavior;

    [CascadingParameter] public BlazorDiagram BlazorDiagram { get; set; } = null!;

    [Parameter] public string Background { get; set; } = "rgb(110 159 212 / 25%)";

    public void Dispose()
    {
        if (_selectionBoxBehavior is not null)
        {
            _selectionBoxBehavior.SelectionBoundsChanged -= SelectionBoundsChanged;
        }
    }

    protected override void OnInitialized()
    {
        _selectionBoxBehavior = BlazorDiagram.GetBehavior<SelectionBoxBehavior>();
        if (_selectionBoxBehavior is not null)
        {
            _selectionBoxBehavior.SelectionBoundsChanged += SelectionBoundsChanged;
        }
    }

    void SelectionBoundsChanged(object? sender, Rectangle? bounds)
    {
        _selectionBounds = bounds;
        InvokeAsync(StateHasChanged);
    }

    private string GenerateStyle()
    {
        return FormattableString.Invariant(
            $"position: absolute; background: {Background}; top: {_selectionBounds!.Top}px; left: {_selectionBounds.Left}px; width: {_selectionBounds.Width}px; height: {_selectionBounds.Height}px;");
    }
}