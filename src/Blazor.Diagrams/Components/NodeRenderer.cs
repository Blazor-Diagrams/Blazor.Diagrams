using Blazor.Diagrams.Core;
using Blazor.Diagrams.Core.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Blazor.Diagrams.Components
{
    public class NodeRenderer : ComponentBase
    {
        [CascadingParameter(Name = "DiagramManager")]
        public DiagramManager DiagramManager { get; set; }

        [Parameter]
        public NodeModel Node { get; set; }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            var componentType = DiagramManager.GetComponentForModel(Node) ?? typeof(NodeWidget);
            builder.OpenComponent(0, componentType);
            builder.AddAttribute(1, "Node", Node);
            builder.CloseComponent();
        }
    }
}
