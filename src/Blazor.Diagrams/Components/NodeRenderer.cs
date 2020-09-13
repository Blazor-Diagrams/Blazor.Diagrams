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
        private bool _isVisible = true;

        [CascadingParameter(Name = "DiagramManager")]
        public DiagramManager DiagramManager { get; set; }

        [Parameter]
        public NodeModel Node { get; set; }

        public void Dispose()
        {
            DiagramManager.PanChanged -= DiagramManager_PanChanged;
            Node.Changed -= Node_Changed;
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            if (!_isVisible)
                return;

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

            DiagramManager.PanChanged += DiagramManager_PanChanged;
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

        private void DiagramManager_PanChanged()
        {
            if (Node.Size == null)
                return;

            var left = Node.Position.X * DiagramManager.Zoom + DiagramManager.Pan.X;
            var top = Node.Position.Y * DiagramManager.Zoom + DiagramManager.Pan.Y;
            var right = left + Node.Size.Width * DiagramManager.Zoom;
            var bottom = top + Node.Size.Height * DiagramManager.Zoom;

            var isVisible = right > 0 && left < DiagramManager.Container.Width && bottom > 0 &&
                top < DiagramManager.Container.Height;

            if (_isVisible != isVisible)
            {
                _isVisible = isVisible;
                _reRender = true;
                StateHasChanged();
            }
        }

        private void Node_Changed()
        {
            _reRender = true;
        }
    }
}