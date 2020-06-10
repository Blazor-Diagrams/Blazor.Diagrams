using Blazor.Diagrams.Core;
using Blazor.Diagrams.Core.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Blazor.Diagrams.Components
{
    public class LinkRenderer : ComponentBase
    {
        [CascadingParameter(Name = "DiagramManager")]
        public DiagramManager DiagramManager { get; set; }

        [Parameter]
        public LinkModel Link { get; set; }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            var componentType = DiagramManager.GetComponentForModel(Link) ?? typeof(LinkWidget);
            builder.OpenComponent(0, componentType);
            builder.AddAttribute(1, "Link", Link);
            builder.CloseComponent();
        }
    }
}
