using System;
using System.Linq;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using SvgPathProperties;

namespace Blazor.Diagrams.Components.Renderers;

public class LinkLabelRenderer : ComponentBase, IDisposable
{
    [CascadingParameter] public BlazorDiagram BlazorDiagram { get; set; } = null!;
    [Parameter] public LinkLabelModel Label { get; set; } = null!;
    [Parameter] public SvgPath[] Paths { get; set; } = null!;

    public void Dispose()
    {
        Label.Changed -= OnLabelChanged;
    }

    protected override void OnInitialized()
    {
        Label.Changed += OnLabelChanged;
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        var component = BlazorDiagram.GetComponent(Label) ?? typeof(DefaultLinkLabelWidget);
        var position = FindPosition();
        if (position == null)
            return;

        builder.OpenComponent(0, component);
        builder.AddAttribute(1, "Label", Label);
        builder.AddAttribute(2, "Position", position);
        builder.CloseComponent();
    }

    private void OnLabelChanged(Model _)
    {
        InvokeAsync(StateHasChanged);
    }

    private Point? FindPosition()
    {
        var totalLength = Paths.Sum(p => p.Length);

        var length = Label.Distance switch
        {
            var d when d >= 0 && d <= 1 => Label.Distance.Value * totalLength,
            var d when d > 1 => Label.Distance.Value,
            var d when d < 0 => totalLength + Label.Distance.Value,
            _ => totalLength * (Label.Parent.Labels.IndexOf(Label) + 1) / (Label.Parent.Labels.Count + 1)
        };

        foreach (var path in Paths)
        {
            var pathLength = path.Length;
            if (length < pathLength)
            {
                var pt = path.GetPointAtLength(length);
                return new Point(pt.X, pt.Y);
            }

            length -= pathLength;
        }

        return null;
    }
}