using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace Blazor.Diagrams.Components
{
    public partial class DiagramCanvas : IDisposable
    {
        [CascadingParameter]
        public Diagram Diagram { get; set; }

        [Parameter]
        public RenderFragment Widgets { get; set; }

        [Parameter]
        public string Class { get; set; }

        [Inject]
        public IJSRuntime JSRuntime { get; set; }

        protected ElementReference elementReference;
        private DotNetObjectReference<DiagramCanvas> _reference;
        private bool _shouldRender;

        private string LayerStyle
            => FormattableString.Invariant($"transform: translate({Diagram.Pan.X}px, {Diagram.Pan.Y}px) scale({Diagram.Zoom});");

        protected override void OnInitialized()
        {
            base.OnInitialized();

            _reference = DotNetObjectReference.Create(this);
            Diagram.Changed += OnDiagramChanged;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
            {
                Diagram.SetContainer(await JSRuntime.GetBoundingClientRect(elementReference));
                await JSRuntime.ObserveResizes(elementReference, _reference);
            }
        }

        [JSInvokable]
        public void OnResize(Rectangle rect) => Diagram.SetContainer(rect);

        protected override bool ShouldRender()
        {
            if (_shouldRender)
            {
                _shouldRender = false;
                return true;
            }

            return false;
        }

        private void OnMouseDown(MouseEventArgs e) => Diagram.OnMouseDown(null, e.ToCore());

        private void OnMouseMove(MouseEventArgs e) => Diagram.OnMouseMove(null, e.ToCore());

        private void OnMouseUp(MouseEventArgs e) => Diagram.OnMouseUp(null, e.ToCore());

        private void OnKeyDown(KeyboardEventArgs e) => Diagram.OnKeyDown(e.ToCore());

        private void OnWheel(WheelEventArgs e) => Diagram.OnWheel(e.ToCore());

        private void OnTouchStart(TouchEventArgs e) => Diagram.OnTouchStart(null, e.ToCore());

        private void OnTouchMove(TouchEventArgs e) => Diagram.OnTouchMove(null, e.ToCore());

        private void OnTouchEnd(TouchEventArgs e) => Diagram.OnTouchEnd(null, e.ToCore());

        private void OnDiagramChanged()
        {
            _shouldRender = true;
            InvokeAsync(StateHasChanged);
        }

        public void Dispose()
        {
            Diagram.Changed -= OnDiagramChanged;

            if (_reference == null)
                return;

            if (elementReference.Id != null)
                _ = JSRuntime.UnobserveResizes(elementReference);

            _reference.Dispose();
        }
    }
}
