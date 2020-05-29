using Blazor.Diagrams.Core.Models;
using Microsoft.AspNetCore.Components;
using System;

namespace Blazor.Diagrams.Components.Base
{
    public class LinkWidgetBaseComponent : ComponentBase, IDisposable
    {
        [Parameter]
        public Link Link { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            Link.Changed += OnLinkChanged;
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
