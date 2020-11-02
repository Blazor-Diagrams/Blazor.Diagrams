using Blazor.Diagrams.Core;
using Blazor.Diagrams.Core.Models.Core;
using Blazor.Diagrams.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace Blazor.Diagrams.Components
{
    public class DiagramCanvasComponent : ComponentBase, IDisposable
    {

        [CascadingParameter(Name = "DiagramManager")]
        public DiagramManager DiagramManager { get; set; }

        [Parameter]
        public RenderFragment Widgets { get; set; }

        [Parameter]
        public string Class { get; set; }

        [Inject]
        public IJSRuntime JSRuntime { get; set; }

        protected ElementReference elementReference;
        private DotNetObjectReference<DiagramCanvasComponent> _reference;
        private bool _shouldReRender;

        public string PanX => DiagramManager.Pan.X.ToString(CultureInfo.InvariantCulture);
        public string PanY => DiagramManager.Pan.Y.ToString(CultureInfo.InvariantCulture);
        public string Zoom => DiagramManager.Zoom.ToString(CultureInfo.InvariantCulture);

        protected override void OnInitialized()
        {
            base.OnInitialized();

            _reference = DotNetObjectReference.Create(this);
            DiagramManager.Changed += DiagramManager_Changed;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
            {
                DiagramManager.Container = await JSRuntime.GetBoundingClientRect(elementReference);
                await JSRuntime.ObserveResizes(elementReference, _reference);
            }
        }

        [JSInvokable]
        public void OnResize(Rectangle rect) => DiagramManager.ChangeContainer(rect);

        protected override bool ShouldRender()
        {
            if (_shouldReRender)
            {
                _shouldReRender = false;
                return true;
            }

            return false;
        }

        protected void OnMouseDown(MouseEventArgs e) => DiagramManager.OnMouseDown(null, e);

        protected void OnMouseMove(MouseEventArgs e) => DiagramManager.OnMouseMove(null, e);

        protected void OnMouseUp(MouseEventArgs e) => DiagramManager.OnMouseUp(null, e);

        protected void OnKeyDown(KeyboardEventArgs e) => DiagramManager.OnKeyDown(e);

        protected void OnWheel(WheelEventArgs e) => DiagramManager.OnWheel(e);

        private void DiagramManager_Changed()
        {
            _shouldReRender = true;
            StateHasChanged();
        }

        public void Dispose()
        {
            DiagramManager.Changed -= DiagramManager_Changed;

            if (_reference == null)
                return;

            if (elementReference.Id != null)
                _ = JSRuntime.UnobserveResizes(elementReference);

            _reference.Dispose();
        }
    }
}
