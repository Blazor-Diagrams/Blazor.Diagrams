using Blazor.Diagrams.Core;
using Blazor.Diagrams.Core.Extensions;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace Blazor.Diagrams.Components.Renderers
{
    public class NodeLabelRenderer : ComponentBase, IDisposable
    {
        [CascadingParameter] public Diagram Diagram { get; set; }
        [Parameter] public NodeLabelModel Label { get; set; }

        [Inject]
        private IJSRuntime JsRuntime { get; set; }

        public void Dispose()
        {
            Label.Changed -= OnLabelChanged;
        }

        protected override void OnInitialized()
        {
            Label.Changed += OnLabelChanged;             
        }

        protected override async Task OnParametersSetAsync()
        {
            Label.Size = await JsRuntime.GetSizeForLabel(Label.Content);
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            var position = FindPosition();
            if (position == null)
                return;

            if (Label.Parent.Layer == RenderLayer.HTML)
            {
                builder.OpenElement(0, "div");
                builder.AddAttribute(1, "class", "nodelabel");
                builder.AddAttribute(2, "style", $"top: {position.Y.ToInvariantString()}px; left: {position.X.ToInvariantString()}px;");
            }
            else
            {
                builder.OpenElement(0, "g");
                builder.AddAttribute(1, "class", "nodelabel");
                builder.OpenElement(2, "foreignObject");
                builder.AddAttribute(3, "x", $"{position.X.ToInvariantString()}");
                builder.AddAttribute(4, "y", $"{position.Y.ToInvariantString()}");
                builder.OpenElement(5, "div");
            }
            builder.AddAttribute(10, "onmousedown", EventCallback.Factory.Create<MouseEventArgs>(this, OnMouseDown));
            builder.AddEventStopPropagationAttribute(11, "onmousedown", true);
            builder.AddAttribute(12, "onmouseup", EventCallback.Factory.Create<MouseEventArgs>(this, OnMouseUp));
            builder.AddEventStopPropagationAttribute(13, "onmouseup", true);
            builder.AddAttribute(14, "ontouchstart", EventCallback.Factory.Create<TouchEventArgs>(this, OnTouchStart));
            builder.AddEventStopPropagationAttribute(15, "ontouchstart", true);
            builder.AddAttribute(16, "ontouchend", EventCallback.Factory.Create<TouchEventArgs>(this, OnTouchEnd));
            builder.AddEventStopPropagationAttribute(17, "ontouchend", true);
            builder.AddEventPreventDefaultAttribute(18, "ontouchend", true);
            builder.AddContent(19, Label.Content);
            if (Label.Parent.Layer == RenderLayer.SVG)
            {
                builder.CloseElement();//foreignObject
                builder.CloseElement();//div
            }
            builder.CloseElement();//g/div
        }

        private void OnLabelChanged() => StateHasChanged();

        private Point FindPosition()
        {
            if(Label.Size != null)
            {
                var lh = Label.Size.Height;
                var lw = Label.Size.Width;
                double nh = 10;
                if(Label.Parent.Size != null) nh = Label.Parent.Size.Height;
                double nw = 10;
                if (Label.Parent.Size != null) nw = Label.Parent.Size.Width;
                double x, y;
                switch (Label.Align)
                {
                    case Alignment.Center:
                        x = (nw - lw) / 2;
                        y = (nh - lh) / 2;
                        break;
                    case Alignment.Top:
                        x = (nw - lw) / 2;
                        y = -lh;
                        break;
                    case Alignment.Right:
                        x = nw;
                        y = (nh - lh) / 2;
                        break;
                    case Alignment.Bottom:
                        x = (nw - lw) / 2;
                        y = nh;
                        break;
                    default:
                        x = -lw;
                        y = (nh - lh) / 2;
                        break;
                }
                x += Label.Parent.Position.X;
                y += Label.Parent.Position.Y;
                if (Label.Position != null)
                {
                    x += Label.Position.X;
                    y += Label.Position.Y;
                }
                return new Point(x, y);
            }

            return null;
        }
        private void OnMouseDown(MouseEventArgs e) => Diagram.OnMouseDown(Label, e);

        private void OnMouseUp(MouseEventArgs e) => Diagram.OnMouseUp(Label, e);

        private void OnTouchStart(TouchEventArgs e) => Diagram.OnTouchStart(Label, e);

        private void OnTouchEnd(TouchEventArgs e) => Diagram.OnTouchEnd(Label, e);
    }
}
