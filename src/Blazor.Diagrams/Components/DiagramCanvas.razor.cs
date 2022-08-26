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
        public Diagram Diagram { get; set; } = null!;

        [Parameter]
        public RenderFragment? Widgets { get; set; }

        [Parameter]
        public string? Class { get; set; }

        [Inject]
        public IJSRuntime JSRuntime { get; set; } = null!;

        protected ElementReference elementReference;
        private DotNetObjectReference<DiagramCanvas>? _reference;
        private bool _shouldRender;

        private string GetLayerStyle(int order)
        {
            return FormattableString.Invariant($"transform: translate({Diagram.Pan.X}px, {Diagram.Pan.Y}px) scale({Diagram.Zoom}); z-index: {order};");
        }

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
                await JSRuntime.ObserveResizes(elementReference, _reference!);
            }
        }

        [JSInvokable]
        public void OnResize(Rectangle rect) => Diagram.SetContainer(rect);

        protected override bool ShouldRender()
        {
            if (!_shouldRender) return false;
            
            _shouldRender = false;
            return true;
        }

        private void OnPointerDown(PointerEventArgs e) => Diagram.TriggerPointerDown(null, e.ToCore());

        private void OnPointerMove(PointerEventArgs e) => Diagram.TriggerPointerMove(null, e.ToCore());

        private void OnPointerUp(PointerEventArgs e) => Diagram.TriggerPointerUp(null, e.ToCore());

        private void OnKeyDown(KeyboardEventArgs e) => Diagram.OnKeyDown(e.ToCore());

        private void OnWheel(WheelEventArgs e) => Diagram.OnWheel(e.ToCore());

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
