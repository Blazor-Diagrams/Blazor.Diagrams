using Blazor.Diagrams.Core;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Core;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;

namespace Blazor.Diagrams.Components.Groups
{
    public partial class GroupContainer : IDisposable
    {
        private bool _shouldRender = true;
        private Size _lastSize;

        [Parameter]
        public GroupModel Group { get; set; }

        [Parameter]
        public string Class { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [CascadingParameter]
        public Diagram Diagram { get; set; }

        public void Dispose()
        {
            Group.Changed -= OnGroupChanged;
        }

        protected override void OnInitialized()
        {
            Group.Changed += OnGroupChanged;
        }

        protected override bool ShouldRender()
        {
            if (_shouldRender)
            {
                _shouldRender = false;
                return true;
            }

            return false;
        }

        protected override void OnAfterRender(bool firstRender)
        {
            if (Size.Zero.Equals(Group.Size))
                return;

            // Update the port positions (and links) when the size of the group changes
            // This will save us some JS trips as well as useless rerenders

            if (_lastSize == null || !_lastSize.Equals(Group.Size))
            {
                Group.ReinitializePorts();
                _lastSize = Group.Size;
            }
        }

        private void OnGroupChanged()
        {
            _shouldRender = true;
            StateHasChanged();
        }

        private void OnMouseDown(MouseEventArgs e) => Diagram.OnMouseDown(Group, e);

        private void OnMouseUp(MouseEventArgs e) => Diagram.OnMouseUp(Group, e);
    }
}
