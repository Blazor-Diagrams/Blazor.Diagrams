using Blazor.Diagrams.Core;
using Blazor.Diagrams.Core.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;

namespace Blazor.Diagrams.Components.Groups
{
    public partial class GroupContainer : IDisposable
    {
        private bool _shouldRender = true;

        [Parameter]
        public GroupModel Group { get; set; }

        [Parameter]
        public string Class { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [CascadingParameter(Name = "DiagramManager")]
        public DiagramManager DiagramManager { get; set; }

        public void Dispose()
        {
            Group.Changed -= OnGroupChanged;
        }

        protected override void OnParametersSet()
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

        private void OnGroupChanged()
        {
            _shouldRender = true;
            StateHasChanged();
        }

        private void OnMouseDown(MouseEventArgs e) => DiagramManager.OnMouseDown(Group, e);

        private void OnMouseUp(MouseEventArgs e) => DiagramManager.OnMouseUp(Group, e);
    }
}
