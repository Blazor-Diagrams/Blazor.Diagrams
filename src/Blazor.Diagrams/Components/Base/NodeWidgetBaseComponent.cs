using Blazor.Diagrams.Core;
using Blazor.Diagrams.Core.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;

namespace Blazor.Diagrams.Components.Base
{
    public class NodeWidgetBaseComponent : ComponentBase, IDisposable
    {
        [CascadingParameter(Name = "DiagramManager")]
        public DiagramManager DiagramManager { get; set; }

        [Parameter]
        public NodeModel Node { get; set; }

        protected ElementReference element;

        protected override void OnInitialized()
        {
            base.OnInitialized();

            Node.Changed += OnNodeChanged;
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
