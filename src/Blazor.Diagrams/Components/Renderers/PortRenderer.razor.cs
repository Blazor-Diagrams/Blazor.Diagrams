using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;
using Blazor.Diagrams.Extensions;
using Blazor.Diagrams.Core.Models.Core;

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

            if (!Port.Initialized || firstRender)
            {
                var zoom = DiagramManager.Zoom;
                var pan = DiagramManager.Pan;
                var rect = await JSRuntime.GetBoundingClientRect(_element);

                Port.Size = new Size(rect.Width / zoom, rect.Height / zoom);
                Port.Position = new Point((rect.Left - DiagramManager.Container.Left - pan.X) / zoom,
                    (rect.Top - DiagramManager.Container.Top - pan.Y) / zoom);

                //if (Port.Parent.Group != null)
                //{
                //    Port.Position -= Port.Parent.Group.MovedBy;
                //}

                Port.Initialized = true;
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
