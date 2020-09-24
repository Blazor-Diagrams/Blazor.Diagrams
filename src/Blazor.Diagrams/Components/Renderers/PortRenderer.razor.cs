using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;
using Blazor.Diagrams.Extensions;

namespace Blazor.Diagrams.Components.Renderers
{
    public partial class PortRenderer : IDisposable
    {
        private bool _shouldRender = true;
        private ElementReference _element;

        [CascadingParameter(Name = "DiagramManager")]
        public DiagramManager DiagramManager { get; set; }

        [Inject]
        private IJSRuntime JSRuntime { get; set; }

        [Parameter]
        public PortModel Port { get; set; }

        [Parameter]
        public string Class { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        public void Dispose()
        {
            Port.Changed -= OnPortChanged;
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            Port.Changed += OnPortChanged;
        }

        protected override bool ShouldRender() => _shouldRender;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            _shouldRender = false;

            if (firstRender)
            {
                var offsetAndSize = await JSRuntime.GetOffsetWithSize(_element);
                Port.Offset = new Point(offsetAndSize[0], offsetAndSize[1]);
                Port.Size = new Size(offsetAndSize[2], offsetAndSize[3]);
                Port.Position = new Point(Port.Parent.Position.X + Port.Offset.X, Port.Parent.Position.Y + Port.Offset.Y);
                Port.RefreshAll();
            }
        }

        protected virtual void OnMouseDown(MouseEventArgs e) => DiagramManager.OnMouseDown(Port, e);

        protected virtual void OnMouseUp(MouseEventArgs e) => DiagramManager.OnMouseUp(Port, e);

        private void OnPortChanged()
        {
            _shouldRender = true;
            StateHasChanged();
        }
    }
}
