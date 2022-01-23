using Blazor.Diagrams.Core;
using Blazor.Diagrams.Core.Extensions;
using Blazor.Diagrams.Core.Geometry;
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
        private bool _shouldRender;
        private bool _isVisible = true;
        private bool _becameVisible;
        private ElementReference _element;
        private DotNetObjectReference<NodeRenderer> _reference;

        [CascadingParameter]
        public Diagram Diagram { get; set; }

        [Parameter]
        public NodeModel Node { get; set; }

        [Inject]
        private IJSRuntime JsRuntime { get; set; }

        public void Dispose()
        {
            Diagram.PanChanged -= CheckVisibility;
            Diagram.ZoomChanged -= CheckVisibility;
            Diagram.ContainerChanged -= CheckVisibility;
            Node.Changed -= ReRender;

            if (_element.Id != null)
                _ = JsRuntime.UnobserveResizes(_element);

            _reference?.Dispose();
        }

        [JSInvokable]
        public void OnResize(Size size)
        {
            // When the node becomes invisible (a.k.a unrendered), the size is zero
            if (Size.Zero.Equals(size))
                return;

            size = new Size(size.Width / Diagram.Zoom, size.Height / Diagram.Zoom);
            if (Node.Size != null && Node.Size.Width.AlmostEqualTo(size.Width) && Node.Size.Height.AlmostEqualTo(size.Height))
                return;

            Node.Size = size;
            Node.Refresh();
            Node.RefreshLinks();
            Node.ReinitializePorts();
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            _reference = DotNetObjectReference.Create(this);
            Diagram.PanChanged += CheckVisibility;
            Diagram.ZoomChanged += CheckVisibility;
            Diagram.ContainerChanged += CheckVisibility;
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

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            if (!_isVisible)
                return;

            var componentType = Diagram.GetComponentForModel(Node) ??
                Diagram.Options.DefaultNodeComponent ??
                (Node.Layer == RenderLayer.HTML ? typeof(NodeWidget) : typeof(SvgNodeWidget));

            builder.OpenElement(0, Node.Layer == RenderLayer.HTML ? "div" : "g");
            builder.AddAttribute(1, "class", $"node{(Node.Locked ? " locked" : string.Empty)}");
            builder.AddAttribute(2, "data-node-id", Node.Id);

            if (Node.Layer == RenderLayer.HTML)
            {
                builder.AddAttribute(3, "style", $"top: {Node.Position.Y.ToInvariantString()}px; left: {Node.Position.X.ToInvariantString()}px");
            }
            else
            {
                builder.AddAttribute(3, "transform", $"translate({Node.Position.X.ToInvariantString()} {Node.Position.Y.ToInvariantString()})");
            }

            builder.AddAttribute(4, "onmousedown", EventCallback.Factory.Create<MouseEventArgs>(this, OnMouseDown));
            builder.AddEventStopPropagationAttribute(5, "onmousedown", true);
            builder.AddAttribute(6, "onmouseup", EventCallback.Factory.Create<MouseEventArgs>(this, OnMouseUp));
            builder.AddEventStopPropagationAttribute(7, "onmouseup", true);
            builder.AddAttribute(8, "ontouchstart", EventCallback.Factory.Create<TouchEventArgs>(this, OnTouchStart));
            builder.AddEventStopPropagationAttribute(9, "ontouchstart", true);
            builder.AddAttribute(10, "ontouchend", EventCallback.Factory.Create<TouchEventArgs>(this, OnTouchEnd));
            builder.AddEventStopPropagationAttribute(11, "ontouchend", true);
            builder.AddEventPreventDefaultAttribute(12, "ontouchend", true);
            builder.AddElementReferenceCapture(13, value => _element = value);
            builder.OpenComponent(14, componentType);
            builder.AddAttribute(15, "Node", Node);
            builder.CloseComponent();
            builder.CloseElement();
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

        private async void CheckVisibility()
        {
            // _isVisible must be true in case virtualization gets disabled and some nodes are hidden
            if (!Diagram.Options.EnableVirtualization && _isVisible)
                return;

            if (Node.Size == null)
                return;

            var left = Node.Position.X * Diagram.Zoom + Diagram.Pan.X;
            var top = Node.Position.Y * Diagram.Zoom + Diagram.Pan.Y;
            var right = left + Node.Size.Width * Diagram.Zoom;
            var bottom = top + Node.Size.Height * Diagram.Zoom;

            var isVisible = right > 0 && left < Diagram.Container.Width && bottom > 0 &&
                top < Diagram.Container.Height;

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
            InvokeAsync(StateHasChanged);
        }

        private void OnMouseDown(MouseEventArgs e) => Diagram.OnMouseDown(Node, e);

        private void OnMouseUp(MouseEventArgs e) => Diagram.OnMouseUp(Node, e);

        private void OnTouchStart(TouchEventArgs e) => Diagram.OnTouchStart(Node, e);

        private void OnTouchEnd(TouchEventArgs e) => Diagram.OnTouchEnd(Node, e);
    }
}