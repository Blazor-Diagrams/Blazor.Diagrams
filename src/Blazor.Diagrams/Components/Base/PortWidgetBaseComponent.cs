using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace Blazor.Diagrams.Components.Base
{
    public class PortWidgetBaseComponent : ComponentBase, IDisposable
    {
        [Inject]
        private IJSRuntime JSRuntime { get; set; }

        [Parameter]
        public Port Port { get; set; }

        protected ElementReference element;

        protected override void OnInitialized()
        {
            base.OnInitialized();

            Port.Changed += OnPortChanged;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
            {
                var offsetAndSize = await JSRuntime.GetOffsetWithSize(element);
                Port.Offset = new Point(offsetAndSize[0], offsetAndSize[1]);
                Port.Size = new Size(offsetAndSize[2], offsetAndSize[3]);
                Port.Position = new Point(Port.Position.X + Port.Offset.X, Port.Position.Y + Port.Offset.Y);
                Port.RefreshAll();
            }
        }

        private void OnPortChanged()
        {
            StateHasChanged();
        }

        public void Dispose()
        {
            Port.Changed -= OnPortChanged;
        }
    }
}
