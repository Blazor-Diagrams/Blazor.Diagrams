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
            var componentType = DiagramManager.GetComponentForModel(Node) ??
                DiagramManager.Options.DefaultNodeComponent ??
                (Node.Layer == RenderLayer.HTML ? typeof(NodeWidget) : typeof(SvgNodeWidget));

            builder.OpenComponent(0, componentType);
            builder.AddAttribute(1, "Node", Node);
            builder.CloseComponent();
        }
    }
}
