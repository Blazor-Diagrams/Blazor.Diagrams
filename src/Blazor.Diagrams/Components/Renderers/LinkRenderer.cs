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
        public Diagram Diagram { get; set; }

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
            var componentType = Diagram.GetComponentForModel(Link) ?? typeof(LinkWidget);

            builder.OpenElement(0, "g");
            builder.AddAttribute(1, "class", "link");
            builder.AddAttribute(2, "data-link-id", Link.Id);
            builder.AddAttribute(3, "onmousedown", EventCallback.Factory.Create<MouseEventArgs>(this, OnMouseDown));
            builder.AddEventStopPropagationAttribute(4, "onmousedown", true);
            builder.AddAttribute(5, "onmouseup", EventCallback.Factory.Create<MouseEventArgs>(this, OnMouseUp));
            builder.AddEventStopPropagationAttribute(6, "onmouseup", true);
            builder.AddAttribute(7, "ontouchstart", EventCallback.Factory.Create<TouchEventArgs>(this, OnTouchStart));
            builder.AddEventStopPropagationAttribute(8, "ontouchstart", true);
            builder.AddAttribute(9, "ontouchend", EventCallback.Factory.Create<TouchEventArgs>(this, OnTouchEnd));
            builder.AddEventStopPropagationAttribute(10, "ontouchend", true);
            builder.AddEventPreventDefaultAttribute(11, "ontouchend", true);
            builder.OpenComponent(12, componentType);
            builder.AddAttribute(13, "Link", Link);
            builder.CloseComponent();
            builder.CloseElement();
        }

        protected override void OnAfterRender(bool firstRender) => _shouldRender = false;

        private void OnLinkChanged()
        {
            _shouldRender = true;
            InvokeAsync(StateHasChanged);
        }

        private void OnMouseDown(MouseEventArgs e) => Diagram.OnMouseDown(Link, e.ToCore());

        private void OnMouseUp(MouseEventArgs e) => Diagram.OnMouseUp(Link, e.ToCore());

        private void OnTouchStart(TouchEventArgs e) => Diagram.OnTouchStart(Link, e.ToCore());

        private void OnTouchEnd(TouchEventArgs e) => Diagram.OnTouchEnd(Link, e.ToCore());
    }
}
