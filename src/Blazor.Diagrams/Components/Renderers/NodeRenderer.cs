using Blazor.Diagrams.Core;
using Blazor.Diagrams.Core.Extensions;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Core;
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
        private bool _shouldRender;
        private bool _isVisible = true;
        private bool _becameVisible;
        private ElementReference _element;
        private DotNetObjectReference<NodeRenderer> _reference;

        [CascadingParameter(Name = "DiagramManager")]
        public DiagramManager DiagramManager { get; set; }

        [Parameter]
        public NodeModel Node { get; set; }

        [Inject]
        private IJSRuntime JsRuntime { get; set; }

        public void Dispose()
        {
            DiagramManager.PanChanged -= DiagramManager_PanChanged;
            Node.Changed -= ReRender;

            if (_reference == null)
                return;

            if (_element.Id != null)
                _ = JsRuntime.UnobserveResizes(_element);

            _reference.Dispose();
        }

        [JSInvokable]
        public void OnResize(Size size)
        {
            // When the node becomes invisible (a.k.a unrendered), the size is zero
            if (Size.Zero.Equals(size))
                return;

            Node.Size = size;
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
            builder.AddAttribute(5, "onmouseup", EventCallback.Factory.Create<MouseEventArgs>(this, OnMouseUp));
            builder.AddEventStopPropagationAttribute(6, "onmouseup", true);
            builder.AddElementReferenceCapture(7, value => _element = value);
            builder.OpenComponent(8, componentType);
            builder.AddAttribute(9, "Node", Node);
            builder.CloseComponent();
            builder.CloseElement();
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            _reference = DotNetObjectReference.Create(this);
            DiagramManager.PanChanged += DiagramManager_PanChanged;
            Node.Changed += ReRender;
        }

        protected override bool ShouldRender()
        {
            if (_shouldRender)
            {
                _shouldRender = false;
                return true;
            }

            return false;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender || _becameVisible)
            {
                _becameVisible = false;
                await JsRuntime.ObserveResizes(_element, _reference);
            }
        }

        private async void DiagramManager_PanChanged()
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
                _becameVisible = isVisible;

                if (!_isVisible)
                {
                    await JsRuntime.UnobserveResizes(_element);
                }

                ReRender();
            }
        }

        private void ReRender()
        {
            _shouldRender = true;
            StateHasChanged();
        }

        private void OnMouseDown(MouseEventArgs e) => DiagramManager.OnMouseDown(Node, e);

        private void OnMouseUp(MouseEventArgs e) => DiagramManager.OnMouseUp(Node, e);
    }
}