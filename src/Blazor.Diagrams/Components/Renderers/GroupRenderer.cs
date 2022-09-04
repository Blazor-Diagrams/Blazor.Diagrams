using System;
using System.Text;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;
using Blazor.Diagrams.Extensions;
using Blazor.Diagrams.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;

namespace Blazor.Diagrams.Components.Renderers;

public class GroupRenderer : ComponentBase, IDisposable
{
    private bool _isSvg;
    private Size? _lastSize;
    private bool _shouldRender = true;

    [CascadingParameter] public BlazorDiagram BlazorDiagram { get; set; } = null!;

    [Parameter] public GroupModel Group { get; set; } = null!;

    public void Dispose()
    {
        Group.Changed -= OnGroupChanged;
    }

    protected override void OnInitialized()
    {
        Group.Changed += OnGroupChanged;
    }

    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        _isSvg = Group is SvgGroupModel;
    }

    protected override bool ShouldRender()
    {
        if (_shouldRender)
        {
            _shouldRender = false;
            return true;
        }

        return false;
    }

    protected override void OnAfterRender(bool firstRender)
    {
        if (Size.Zero.Equals(Group.Size))
            return;

        // Update the port positions (and links) when the size of the group changes
        // This will save us some JS trips as well as useless rerenders

        if (_lastSize == null || !_lastSize.Equals(Group.Size))
        {
            Group.ReinitializePorts();
            Group.RefreshLinks();
            _lastSize = Group.Size;
        }
    }

    private void OnGroupChanged(Model _)
    {
        _shouldRender = true;
        InvokeAsync(StateHasChanged);
    }

    private static string GenerateStyle(double top, double left, double width, double height)
    {
        return FormattableString.Invariant($"top: {top}px; left: {left}px; width: {width}px; height: {height}px");
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        var componentType = BlazorDiagram.GetComponentForModel(Group) ?? typeof(DefaultGroupWidget);
        var classes = new StringBuilder("group")
            .AppendIf(" locked", Group.Locked)
            .AppendIf(" selected", Group.Selected)
            .AppendIf(" default", componentType == typeof(DefaultGroupWidget));

        builder.OpenElement(0, _isSvg ? "g" : "div");
        builder.AddAttribute(1, "class", classes.ToString());
        builder.AddAttribute(2, "data-group-id", Group.Id);

        if (_isSvg)
            builder.AddAttribute(3, "transform",
                FormattableString.Invariant($"translate({Group.Position.X} {Group.Position.Y})"));
        else
            builder.AddAttribute(3, "style",
                GenerateStyle(Group.Position.Y, Group.Position.X, Group.Size!.Width, Group.Size.Height));

        builder.AddAttribute(4, "onpointerdown", EventCallback.Factory.Create<PointerEventArgs>(this, OnPointerDown));
        builder.AddEventStopPropagationAttribute(5, "onpointerdown", true);
        builder.AddAttribute(6, "onpointerup", EventCallback.Factory.Create<PointerEventArgs>(this, OnPointerUp));
        builder.AddEventStopPropagationAttribute(7, "onpointerup", true);

        if (_isSvg)
        {
            builder.OpenElement(8, "rect");
            builder.AddAttribute(9, "width", Group.Size!.Width);
            builder.AddAttribute(10, "height", Group.Size.Height);
            builder.AddAttribute(11, "fill", "none");
            builder.CloseElement();
        }

        builder.OpenComponent(12, componentType);
        builder.AddAttribute(13, "Group", Group);
        builder.CloseComponent();
        builder.CloseElement();
    }

    private void OnPointerDown(PointerEventArgs e)
    {
        BlazorDiagram.TriggerPointerDown(Group, e.ToCore());
    }

    private void OnPointerUp(PointerEventArgs e)
    {
        BlazorDiagram.TriggerPointerUp(Group, e.ToCore());
    }
}