using Blazor.Diagrams.Core;
using Blazor.Diagrams.Core.Models.Core;
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
        private bool _shouldReRender;

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
                Diagram.Container = await JSRuntime.GetBoundingClientRect(elementReference);
                await JSRuntime.ObserveResizes(elementReference, _reference);
            }
        }

        [JSInvokable]
        public void OnResize(Rectangle rect) => Diagram.SetContainer(rect);

        protected override bool ShouldRender()
        {
            if (_shouldReRender)
            {
                _shouldReRender = false;
                return true;
            }

            return false;
        }

        protected void OnMouseDown(MouseEventArgs e) => Diagram.OnMouseDown(null, e);

        protected void OnMouseMove(MouseEventArgs e) => Diagram.OnMouseMove(null, e);

        protected void OnMouseUp(MouseEventArgs e) => Diagram.OnMouseUp(null, e);

        protected void OnKeyDown(KeyboardEventArgs e) => Diagram.OnKeyDown(e);

        protected void OnWheel(WheelEventArgs e) => Diagram.OnWheel(e);

        private void OnDiagramChanged()
        {
            _shouldReRender = true;
            StateHasChanged();
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
