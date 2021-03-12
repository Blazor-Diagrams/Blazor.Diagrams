using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;
using Blazor.Diagrams.Extensions;
using Blazor.Diagrams.Core.Geometry;

namespace Blazor.Diagrams.Components.Renderers
{
    public partial class PortRenderer : IDisposable
    {
        private bool _shouldRender = true;
        private ElementReference _element;

        [CascadingParameter]
        public Diagram Diagram { get; set; }

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

            if (!Port.Initialized)
            {
                await UpdateDimensions();
            }
        }

        protected virtual void OnMouseDown(MouseEventArgs e) => Diagram.OnMouseDown(Port, e);

        protected virtual void OnMouseUp(MouseEventArgs e) => Diagram.OnMouseUp(Port, e);

        private async Task UpdateDimensions()
        {
            var zoom = Diagram.Zoom;
            var pan = Diagram.Pan;
            var rect = await JSRuntime.GetBoundingClientRect(_element);

            Port.Size = new Size(rect.Width / zoom, rect.Height / zoom);
            Port.Position = new Point((rect.Left - Diagram.Container.Left - pan.X) / zoom,
                (rect.Top - Diagram.Container.Top - pan.Y) / zoom);

            Port.Initialized = true;

            // We don't really need to refresh the port again,
            // let's just refresh the links so that they use the new port's position
            Port.RefreshLinks();
        }

        private async void OnPortChanged()
        {
            if (Port.Initialized)
            {
                _shouldRender = true;
                StateHasChanged();
            }
            else
            {
                await UpdateDimensions();
            }
        }
    }
}
