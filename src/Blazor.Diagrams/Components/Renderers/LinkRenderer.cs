using Blazor.Diagrams.Core;
using Blazor.Diagrams.Core.Models;
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
        public LinkModel Link { get; set; }

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
            builder.AddAttribute(2, "onmousedown", EventCallback.Factory.Create<MouseEventArgs>(this, OnMouseDown));
            builder.AddEventStopPropagationAttribute(3, "onmousedown", true);
            builder.AddAttribute(4, "onmouseup", EventCallback.Factory.Create<MouseEventArgs>(this, OnMouseUp));
            builder.AddEventStopPropagationAttribute(5, "onmouseup", true);
            builder.OpenComponent(6, componentType);
            builder.AddAttribute(7, "Link", Link);
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
