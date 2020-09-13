using Blazor.Diagrams.Core;
using Blazor.Diagrams.Core.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;

namespace Blazor.Diagrams.Components
{
    public class NodeRenderer : ComponentBase, IDisposable
    {
        private bool _reRender;

        [CascadingParameter(Name = "DiagramManager")]
        public DiagramManager DiagramManager { get; set; }

        [Parameter]
        public NodeModel Node { get; set; }

        public void Dispose()
        {
            Node.Changed -= Node_Changed;
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            var componentType = DiagramManager.GetComponentForModel(Node) ??
                DiagramManager.Options.DefaultNodeComponent ??
                (Node.Layer == RenderLayer.HTML ? typeof(NodeWidget) : typeof(SvgNodeWidget));

            builder.OpenComponent(0, componentType);
            builder.AddAttribute(1, "Node", Node);
            builder.CloseComponent();
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            Node.Changed += Node_Changed;
        }

        protected override bool ShouldRender()
        {
            if (_reRender)
            {
                _reRender = false;
                return true;
            }

            return false;
        }

        private void Node_Changed()
        {
            _reRender = true;
        }
    }
}