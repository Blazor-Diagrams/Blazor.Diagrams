﻿using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Blazor.Diagrams.Components;

public partial class LinkWidget
{
    private bool _hovered;

    [CascadingParameter] public BlazorDiagram BlazorDiagram { get; set; } = null!;
    [Parameter] public LinkModel Link { get; set; } = null!;

    private RenderFragment GetSelectionHelperPath(string color, string d, int index)
    {
        return builder =>
        {
            builder.OpenElement(0, "path");
            builder.AddAttribute(1, "class", "selection-helper");
            builder.AddAttribute(2, "stroke", color);
            builder.AddAttribute(3, "stroke-width", 12);
            builder.AddAttribute(4, "d", d);
            builder.AddAttribute(5, "stroke-linecap", "butt");
            builder.AddAttribute(6, "stroke-opacity", _hovered ? "0.05" : "0");
            builder.AddAttribute(7, "fill", "none");
            builder.AddAttribute(8, "onmouseenter", EventCallback.Factory.Create<MouseEventArgs>(this, OnMouseEnter));
            builder.AddAttribute(9, "onmouseleave", EventCallback.Factory.Create<MouseEventArgs>(this, OnMouseLeave));
            builder.AddAttribute(10, "onpointerdown", EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.PointerEventArgs>(this, e => OnPointerDown(e, index)));
            builder.AddAttribute(10, "ondblclick", EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, e => OnPointerDoubleClick(e, index)));
            builder.AddEventStopPropagationAttribute(11, "onpointerdown", Link.Segmentable);
            builder.CloseElement();
        };
    }

    private void OnPointerDown(PointerEventArgs e, int index)
    {
        if (!Link.Segmentable || Link.DoubleClickToSegment)
            return;

        var vertex = CreateVertex(e.ClientX, e.ClientY, index);
        BlazorDiagram.TriggerPointerDown(vertex, e.ToCore());
    }

    private void OnPointerDoubleClick(MouseEventArgs e, int index)
    {
        if (!Link.Segmentable || !Link.DoubleClickToSegment)
            return;

        var vertex = CreateVertex(e.ClientX, e.ClientY, index);
        BlazorDiagram.TriggerPointerDoubleClick(vertex, e.ToCore());
    }

    private void OnMouseEnter(MouseEventArgs e)
    {
        _hovered = true;
    }

    private void OnMouseLeave(MouseEventArgs e)
    {
        _hovered = false;
    }

    private LinkVertexModel CreateVertex(double clientX, double clientY, int index)
    {
        var rPt = BlazorDiagram.GetRelativeMousePoint(clientX, clientY);
        var vertex = Link.InsertVertex(rPt, index);
        return vertex;
    }
}
