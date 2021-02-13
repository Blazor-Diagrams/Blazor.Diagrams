using Blazor.Diagrams.Core;
using Blazor.Diagrams.Core.Models.Base;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using System;

namespace Blazor.Diagrams.Components.Renderers
{
    public class LinkRenderer : ComponentBase, IDisposable
    {
        private bool _shouldRender = true;

        [CascadingParameter(Name = "DiagramManager")]
        public DiagramManager DiagramManager { get; set; }

        [Parameter]
        public BaseLinkModel Link { get; set; }

        public void Dispose()
        {
            Link.Changed -= Link_Changed;
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            Link.Changed += Link_Changed;
        }

        protected override bool ShouldRender() => _shouldRender;

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            var componentType = DiagramManager.GetComponentForModel(Link) ??
                DiagramManager.Options.Links.DefaultLinkComponent ??
                typeof(LinkWidget);

            builder.OpenElement(0, "g");
            builder.AddAttribute(1, "class", "link");
            builder.AddAttribute(2, "data-link-id", Link.Id);
            builder.AddAttribute(3, "onmousedown", EventCallback.Factory.Create<MouseEventArgs>(this, OnMouseDown));
            builder.AddEventStopPropagationAttribute(4, "onmousedown", true);
            builder.AddAttribute(5, "onmouseup", EventCallback.Factory.Create<MouseEventArgs>(this, OnMouseUp));
            builder.AddEventStopPropagationAttribute(6, "onmouseup", true);
            builder.OpenComponent(7, componentType);
            builder.AddAttribute(8, "Link", Link);
            builder.CloseComponent();
            builder.CloseElement();
        }

        protected override void OnAfterRender(bool firstRender) => _shouldRender = false;

        private void Link_Changed()
        {
            _shouldRender = true;
            StateHasChanged();
        }

        private void OnMouseDown(MouseEventArgs e) => DiagramManager.OnMouseDown(Link, e);

        private void OnMouseUp(MouseEventArgs e) => DiagramManager.OnMouseUp(Link, e);
    }
}
