using Blazor.Diagrams.Core;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace Blazor.Diagrams.Components.Base
{
    public class NodeWidgetBaseComponent<T> : ComponentBase, IDisposable where T : NodeModel
    {
        [CascadingParameter(Name = "DiagramManager")]
        public DiagramManager DiagramManager { get; set; }

        [Parameter]
        public T Node { get; set; }

        [Inject]
        private IJSRuntime jsRuntime { get; set; }

        protected ElementReference element;

        protected override void OnInitialized()
        {
            base.OnInitialized();

            Node.Changed += OnNodeChanged;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
            {
                var rect = await jsRuntime.GetBoundingClientRect(element);
                Node.Size = new Size(rect.Width, rect.Height);
            }
        }

        protected virtual void OnMouseDown(MouseEventArgs e)
        {
            DiagramManager.OnMouseDown(Node, e);
        }

        private void OnNodeChanged()
        {
            StateHasChanged();
        }

        public void Dispose()
        {
            Node.Changed -= OnNodeChanged;
        }
    }
}
