using Blazor.Diagrams.Core;
using Blazor.Diagrams.Core.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;

namespace Blazor.Diagrams.Components.Base
{
    public class LinkWidgetBaseComponent : ComponentBase, IDisposable
    {
        [CascadingParameter(Name = nameof(DiagramManager))]
        public DiagramManager DiagramManager { get; set; }

        [Parameter]
        public LinkModel Link { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            Link.Changed += OnLinkChanged;
        }

        protected virtual void OnMouseDown(MouseEventArgs e)
        {
            DiagramManager.OnMouseDown(Link, e);
        }

        private void OnLinkChanged()
        {
            StateHasChanged();
        }

        public void Dispose()
        {
            Link.Changed -= OnLinkChanged;
        }
    }
}
