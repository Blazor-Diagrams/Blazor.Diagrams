using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;
using Blazor.Diagrams.Extensions;
using Blazor.Diagrams.Core.Geometry;
using Microsoft.AspNetCore.Components.Rendering;
using System.Linq;

namespace Blazor.Diagrams.Components.Renderers
{
    public class PortRenderer : ComponentBase, IDisposable
    {
        private bool _shouldRender = true;
        private ElementReference _element;
        private bool _updatingDimensions;
        private bool _shouldRefreshPort;

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

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            builder.OpenElement(0, Port.Parent.Layer == RenderLayer.HTML ? "div" : "g");
            builder.AddAttribute(1, "class", "port" + " " + (Port.Alignment.ToString().ToLower()) + " " + (Port.Links.Count > 0 ? "has-links" : "") + " " + (Class));
            builder.AddAttribute(2, "data-port-id", Port.Id);
            builder.AddAttribute(3, "onmousedown", EventCallback.Factory.Create<MouseEventArgs>(this, OnMouseDown));
            builder.AddEventStopPropagationAttribute(4, "onmousedown", true);
            builder.AddAttribute(5, "onmouseup", EventCallback.Factory.Create<MouseEventArgs>(this, OnMouseUp));
            builder.AddEventStopPropagationAttribute(6, "onmouseup", true);
            builder.AddAttribute(7, "ontouchstart", EventCallback.Factory.Create<TouchEventArgs>(this, OnTouchStart));
            builder.AddEventStopPropagationAttribute(8, "ontouchstart", true);
            builder.AddAttribute(9, "ontouchend", EventCallback.Factory.Create<TouchEventArgs>(this, OnTouchEnd));
            builder.AddEventStopPropagationAttribute(10, "ontouchend", true);
            builder.AddEventPreventDefaultAttribute(11, "ontouchend", true);
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

        private void OnMouseDown(MouseEventArgs e) => Diagram.OnMouseDown(Port, e.ToCore());

        private void OnMouseUp(MouseEventArgs e) => Diagram.OnMouseUp(Port, e.ToCore());

        private void OnTouchStart(TouchEventArgs e) => Diagram.OnTouchStart(Port, e.ToCore());

        private void OnTouchEnd(TouchEventArgs e)
            => Diagram.OnTouchEnd(FindPortOn(e.ChangedTouches[0].ClientX, e.ChangedTouches[0].ClientY), e.ToCore());

        private PortModel FindPortOn(double clientX, double clientY)
        {
            var allPorts = Diagram.Nodes.SelectMany(n => n.Ports)
                .Union(Diagram.Groups.SelectMany(g => g.Ports));

            foreach (var port in allPorts)
            {
                if (!port.Initialized)
                    continue;

                var relativePt = Diagram.GetRelativeMousePoint(clientX, clientY);
                if (port.GetBounds().ContainsPoint(relativePt))
                    return port;
            }

            return null;
        }

        private async Task UpdateDimensions()
        {
            _updatingDimensions = true;
            var zoom = Diagram.Zoom;
            var pan = Diagram.Pan;
            var rect = await JSRuntime.GetBoundingClientRect(_element);

            Port.Size = new Size(rect.Width / zoom, rect.Height / zoom);
            Port.Position = new Point((rect.Left - Diagram.Container.Left - pan.X) / zoom,
                (rect.Top - Diagram.Container.Top - pan.Y) / zoom);

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
