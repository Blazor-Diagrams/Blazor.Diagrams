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
    public class NodeWidgetBaseComponent : ComponentBase, IDisposable
    {
        [CascadingParameter(Name = "DiagramManager")]
        public DiagramManager DiagramManager { get; set; }

        [Parameter]
        public NodeModel Node { get; set; }

        [Inject]
        private IJSRuntime JSRuntime { get; set; }

        protected ElementReference element;

        protected override void OnInitialized()
        {
            base.OnInitialized();

            Node.Changed += OnNodeChanged;
        }

        protected async Task OnMouseDown(MouseEventArgs e)
        {
            var offsets = await JSRuntime.GetOffset(element);
            Node.LastOffset = new Point(offsets[0] - e.ClientX, offsets[1] - e.ClientY);
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
