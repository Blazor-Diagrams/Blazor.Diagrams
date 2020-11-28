using Blazor.Diagrams.Core;
using Blazor.Diagrams.Core.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;

namespace Blazor.Diagrams.Components.Groups
{
    public partial class GroupContainer : IDisposable
    {
        [Parameter]
        public GroupModel Group { get; set; }

        [Parameter]
        public ushort Padding { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [CascadingParameter(Name = "DiagramManager")]
        public DiagramManager DiagramManager { get; set; }

        public double X => Group.Position.X - Padding;
        public double Y => Group.Position.Y - Padding;
        public double Width => Group.Size.Width + Padding;
        public double Height => Group.Size.Height + Padding;

        public void Dispose()
        {
            Group.Changed -= OnGroupChanged;
        }

        protected override void OnParametersSet()
        {
            Group.Changed += OnGroupChanged;
        }

        private void OnGroupChanged() => StateHasChanged(); // Todo: Use _shouldRender pattern

        private void OnMouseDown(MouseEventArgs e) => DiagramManager.OnMouseDown(Group, e);

        private void OnMouseUp(MouseEventArgs e) => DiagramManager.OnMouseUp(Group, e);
    }
}
