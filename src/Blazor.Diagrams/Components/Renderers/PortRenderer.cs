using Blazor.Diagrams.Core.Models;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;
using Blazor.Diagrams.Extensions;
using Blazor.Diagrams.Core.Geometry;
using Microsoft.AspNetCore.Components.Rendering;
using System.Linq;
using Blazor.Diagrams.Models;

namespace Blazor.Diagrams.Components.Renderers
{
    public class PortRenderer : ComponentBase, IDisposable
    {
        private bool _shouldRender = true;
        private ElementReference _element;
        private bool _updatingDimensions;
        private bool _shouldRefreshPort;
        private bool _isParentSvg;

        [CascadingParameter]
        public BlazorDiagram BlazorDiagram { get; set; } = null!;

        [Inject]
        private IJSRuntime JSRuntime { get; set; } = null!;

        [Parameter]
        public PortModel Port { get; set; } = null!;

        [Parameter]
        public string? Class { get; set; }

        [Parameter]
        public RenderFragment? ChildContent { get; set; }

        public void Dispose()
        {
            Port.Changed -= OnPortChanged;
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            Port.Changed += OnPortChanged;
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            _isParentSvg = Port.Parent is SvgNodeModel;
        }

        protected override bool ShouldRender() => _shouldRender;

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            builder.OpenElement(0, _isParentSvg ? "g" : "div");
            builder.AddAttribute(1, "class", "port" + " " + (Port.Alignment.ToString().ToLower()) + " " + (Port.Links.Count > 0 ? "has-links" : "") + " " + (Class));
            builder.AddAttribute(2, "data-port-id", Port.Id);
            builder.AddAttribute(3, "onpointerdown", EventCallback.Factory.Create<PointerEventArgs>(this, OnPointerDown));
            builder.AddEventStopPropagationAttribute(4, "onpointerdown", true);
            builder.AddAttribute(5, "onpointerup", EventCallback.Factory.Create<PointerEventArgs>(this, OnPointerUp));
            builder.AddEventStopPropagationAttribute(6, "onpointerup", true);
            builder.AddElementReferenceCapture(12, (__value) => { _element = __value; });
            builder.AddContent(13, ChildContent);
            builder.CloseElement();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            _shouldRender = false;

            if (!Port.Initialized)
            {
                await UpdateDimensions();
            }
        }

        private void OnPointerDown(PointerEventArgs e) => BlazorDiagram.TriggerPointerDown(Port, e.ToCore());

        private void OnPointerUp(PointerEventArgs e)
        {
            var model = e.PointerType == "mouse" ? Port : FindPortOn(e.ClientX, e.ClientY);
            BlazorDiagram.TriggerPointerUp(model, e.ToCore());
        }

        private PortModel? FindPortOn(double clientX, double clientY)
        {
            var allPorts = BlazorDiagram.Nodes.SelectMany(n => n.Ports)
                .Union(BlazorDiagram.Groups.SelectMany(g => g.Ports));

            foreach (var port in allPorts)
            {
                if (!port.Initialized)
                    continue;

                var relativePt = BlazorDiagram.GetRelativeMousePoint(clientX, clientY);
                if (port.GetBounds().ContainsPoint(relativePt))
                    return port;
            }

            return null;
        }

        private async Task UpdateDimensions()
        {
            _updatingDimensions = true;
            var zoom = BlazorDiagram.Zoom;
            var pan = BlazorDiagram.Pan;
            var rect = await JSRuntime.GetBoundingClientRect(_element);

            Port.Size = new Size(rect.Width / zoom, rect.Height / zoom);
            Port.Position = new Point((rect.Left - BlazorDiagram.Container.Left - pan.X) / zoom,
                (rect.Top - BlazorDiagram.Container.Top - pan.Y) / zoom);

            Port.Initialized = true;
            _updatingDimensions = false;

            if (_shouldRefreshPort)
            {
                _shouldRefreshPort = false;
                Port.RefreshAll();
            }
            else
            {
                Port.RefreshLinks();
            }
        }

        private async void OnPortChanged()
        {
            // If an update is ongoing and the port is refreshed again,
            // it's highly likely the port needs to be refreshed (e.g. link added)
            if (_updatingDimensions)
            {
                _shouldRefreshPort = true;
            }

            if (Port.Initialized)
            {
                _shouldRender = true;
                await InvokeAsync(StateHasChanged);
            }
            else
            {
                await UpdateDimensions();
            }
        }
    }
}
