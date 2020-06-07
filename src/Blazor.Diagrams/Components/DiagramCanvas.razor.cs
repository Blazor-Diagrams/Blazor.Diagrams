using Blazor.Diagrams.Core;
using Blazor.Diagrams.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace Blazor.Diagrams.Components
{
    public class DiagramCanvasComponent : ComponentBase, IDisposable
    {

        [CascadingParameter(Name = "DiagramManager")]
        public DiagramManager DiagramManager { get; set; }

        [Inject]
        public IJSRuntime JSRuntime { get; set; }

        protected ElementReference elementReference;
        private bool _shouldReRender;

        protected override void OnInitialized()
        {
            base.OnInitialized();

            DiagramManager.Changed += DiagramManager_Changed;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
            {
                DiagramManager.Container = await JSRuntime.GetBoundingClientRect(elementReference);
            }
        }

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

        private void DiagramManager_Changed()
        {
            _shouldReRender = true;
            StateHasChanged();
        }

        public void Dispose()
        {
            DiagramManager.Changed -= DiagramManager_Changed;
        }
    }
}
