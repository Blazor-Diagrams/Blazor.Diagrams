using Blazor.Diagrams.Core;
using Blazor.Diagrams.Core.Extensions;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace Blazor.Diagrams.Components.Renderers
{
    public class NodeRenderer : ComponentBase, IDisposable
    {
        private bool _reRender;
        private bool _isVisible = true;
        private ElementReference _element;

        [CascadingParameter(Name = "DiagramManager")]
        public DiagramManager DiagramManager { get; set; }

        [Parameter]
        public NodeModel Node { get; set; }

        [Inject]
        private IJSRuntime jsRuntime { get; set; }

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

            builder.OpenElement(0, Node.Layer == RenderLayer.HTML ? "div" : "g");
            builder.AddAttribute(1, "class", "node");

            if (Node.Layer == RenderLayer.HTML)
            {
                builder.AddAttribute(2, "style", $"top: {Node.Position.Y.ToInvariantString()}px; left: {Node.Position.X.ToInvariantString()}px");
            }
            else
            {
                builder.AddAttribute(2, "transform", $"translate({Node.Position.X.ToInvariantString()} {Node.Position.Y.ToInvariantString()})");
            }

            builder.AddAttribute(3, "onmousedown", EventCallback.Factory.Create<MouseEventArgs>(this, OnMouseDown));
            builder.AddEventStopPropagationAttribute(4, "onmousedown", true);
            builder.AddAttribute(4, "onmouseup", EventCallback.Factory.Create<MouseEventArgs>(this, OnMouseUp));
            builder.AddEventStopPropagationAttribute(5, "onmouseup", true);
            builder.AddElementReferenceCapture(6, value => _element = value);
            builder.OpenComponent(7, componentType);
            builder.AddAttribute(8, "Node", Node);
            builder.CloseComponent();
            builder.CloseElement();
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

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            // In case the node becomes visible again, no need to get the size
            if (firstRender && Node.Size == null)
            {
                var rect = await jsRuntime.GetBoundingClientRect(_element);
                Node.Size = new Size(rect.Width, rect.Height);
            }
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
            StateHasChanged();
        }

        private void OnMouseDown(MouseEventArgs e) => DiagramManager.OnMouseDown(Node, e);
        private void OnMouseUp(MouseEventArgs e) => DiagramManager.OnMouseUp(Node, e);

    }
}