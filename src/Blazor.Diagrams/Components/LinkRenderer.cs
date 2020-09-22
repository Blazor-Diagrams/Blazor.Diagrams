using Blazor.Diagrams.Core;
using Blazor.Diagrams.Core.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using System;

namespace Blazor.Diagrams.Components
{
    public class LinkRenderer : ComponentBase, IDisposable
    {
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

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            var componentType = DiagramManager.GetComponentForModel(Link) ?? 
                DiagramManager.Options.DefaultLinkComponent ??
                typeof(LinkWidget);

            builder.OpenElement(0, "g");
            builder.AddAttribute(1, "class", "link");
            builder.AddAttribute(2, "onmousedown", EventCallback.Factory.Create<MouseEventArgs>(this, OnMouseDown));
            builder.AddEventStopPropagationAttribute(3, "onmousedown", true);
            builder.OpenComponent(4, componentType);
            builder.AddAttribute(5, "Link", Link);
            builder.CloseComponent();
            builder.CloseElement();
        }

        private void Link_Changed() => StateHasChanged();

        private void OnMouseDown(MouseEventArgs e) => DiagramManager.OnMouseDown(Link, e);
    }
}
