using Blazor.Diagrams.Core;
using Blazor.Diagrams.Core.Models.Base;
using Blazor.Diagrams.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using System;

namespace Blazor.Diagrams.Components.Renderers
{
    public class LinkRenderer : ComponentBase, IDisposable
    {
        private bool _shouldRender = true;

        [CascadingParameter]
        public BlazorDiagram BlazorDiagram { get; set; }

        [Parameter]
        public BaseLinkModel Link { get; set; }

        public void Dispose()
        {
            Link.Changed -= OnLinkChanged;
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            Link.Changed += OnLinkChanged;
        }

        protected override bool ShouldRender() => _shouldRender;

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            var componentType = BlazorDiagram.GetComponentForModel(Link) ?? typeof(LinkWidget);

            builder.OpenElement(0, "g");
            builder.AddAttribute(1, "class", "link");
            builder.AddAttribute(2, "data-link-id", Link.Id);
            builder.AddAttribute(3, "onpointerdown", EventCallback.Factory.Create<PointerEventArgs>(this, OnPointerDown));
            builder.AddEventStopPropagationAttribute(4, "onpointerdown", true);
            builder.AddAttribute(5, "onpointerup", EventCallback.Factory.Create<PointerEventArgs>(this, OnPointerUp));
            builder.AddEventStopPropagationAttribute(6, "onpointerup", true);
            builder.OpenComponent(7, componentType);
            builder.AddAttribute(8, "Link", Link);
            builder.CloseComponent();
            builder.CloseElement();
        }

        protected override void OnAfterRender(bool firstRender) => _shouldRender = false;

        private void OnLinkChanged()
        {
            _shouldRender = true;
            InvokeAsync(StateHasChanged);
        }

        private void OnPointerDown(PointerEventArgs e) => BlazorDiagram.TriggerPointerDown(Link, e.ToCore());

        private void OnPointerUp(PointerEventArgs e) => BlazorDiagram.TriggerPointerUp(Link, e.ToCore());
    }
}
