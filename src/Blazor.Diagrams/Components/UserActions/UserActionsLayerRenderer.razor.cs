using System;
using System.Threading.Tasks;
using Blazor.Diagrams.Core.Extensions;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models.Base;
using Blazor.Diagrams.Core.UserActions;
using Blazor.Diagrams.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Blazor.Diagrams.Components.UserActions;

public partial class UserActionsLayerRenderer : IDisposable
{
    private bool _shouldRender;

    [CascadingParameter] public BlazorDiagram BlazorDiagram { get; set; } = null!;

    [Parameter] public bool Svg { get; set; }

    protected override void OnInitialized()
    {
        BlazorDiagram.UserActions.Changed += OnUserActionsChanged;
    }

    protected override bool ShouldRender()
    {
        if (!_shouldRender)
            return false;

        _shouldRender = false;
        return true;
    }

    private void OnUserActionsChanged(Model cause)
    {
        if ((Svg && cause is SvgNodeModel or SvgGroupModel) ||
            (!Svg && cause is not SvgNodeModel && cause is not SvgGroupModel))
        {
            _shouldRender = true;
            InvokeAsync(StateHasChanged);
        }
    }

    private RenderFragment RenderUserAction(Model model, UserAction action, Point position, bool svg)
    {
        var componentType = BlazorDiagram.GetComponentForModel(action.GetType());
        if (componentType == null)
            throw new BlazorDiagramsException(
                $"A component couldn't be found for the user action {action.GetType().Name}");

        return builder =>
        {
            builder.OpenElement(0, svg ? "g" : "div");
            builder.AddAttribute(1, "class", $"user-action {action.GetType().Name}");
            if (svg)
            {
                builder.AddAttribute(2, "transform",
                    $"translate({position.X.ToInvariantString()} {position.Y.ToInvariantString()})");
            }
            else
            {
                builder.AddAttribute(2, "style",
                    $"top: {position.Y.ToInvariantString()}px; left: {position.X.ToInvariantString()}px");
            }

            builder.AddAttribute(3, "onpointerdown",
                EventCallback.Factory.Create<PointerEventArgs>(this, e => OnPointerDown(e, model, action)));
            builder.AddEventStopPropagationAttribute(4, "onpointerdown", true);

            builder.OpenComponent(5, componentType);
            builder.AddAttribute(6, "Action", action);
            builder.AddAttribute(7, "Model", model);
            builder.CloseComponent();
            builder.CloseElement();
        };
    }

    private async Task OnPointerDown(PointerEventArgs e, Model model, UserAction action)
    {
        await action.Execute(BlazorDiagram, model);
    }

    public void Dispose()
    {
        BlazorDiagram.UserActions.Changed -= OnUserActionsChanged;
    }
}