using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Text;

namespace Blazor.Diagrams.Components.Renderers
{
    public class GroupRenderer : ComponentBase, IDisposable
    {
        private bool _shouldRender = true;
        private Size? _lastSize;
        private bool _isSvg;

        [CascadingParameter]
        public Diagram Diagram { get; set; } = null!;

        [Parameter]
        public GroupModel Group { get; set; } = null!;

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
                Group.RefreshLinks();
                _lastSize = Group.Size;
            }
        }

        private void OnGroupChanged()
        {
            _shouldRender = true;
            InvokeAsync(StateHasChanged);
        }

        private static string GenerateStyle(double top, double left, double width, double height)
        {
            return FormattableString.Invariant($"top: {top}px; left: {left}px; width: {width}px; height: {height}px");
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            var componentType = Diagram.GetComponentForModel(Group) ?? typeof(DefaultGroupWidget);
            var classes = new StringBuilder("group")
                .AppendIf(" locked", Group.Locked)
                .AppendIf(" selected", Group.Selected)
                .AppendIf(" default", componentType == typeof(DefaultGroupWidget));

            builder.OpenElement(0, _isSvg ? "g" : "div");
            builder.AddAttribute(1, "class", classes);
            builder.AddAttribute(2, "data-group-id", Group.Id);

            if (_isSvg)
            {
                // Todo
            }
            else
            {
                builder.AddAttribute(3, "style", GenerateStyle(Group.Position.Y, Group.Position.X, Group.Size!.Width, Group.Size.Height));
            }

            builder.AddAttribute(4, "onmousedown", EventCallback.Factory.Create<MouseEventArgs>(this, OnMouseDown));
            builder.AddEventStopPropagationAttribute(5, "onmousedown", true);
            builder.AddAttribute(6, "onmouseup", EventCallback.Factory.Create<MouseEventArgs>(this, OnMouseUp));
            builder.AddEventStopPropagationAttribute(7, "onmouseup", true);
            builder.AddAttribute(8, "ontouchstart", EventCallback.Factory.Create<TouchEventArgs>(this, OnTouchStart));
            builder.AddEventStopPropagationAttribute(9, "ontouchstart", true);
            builder.AddAttribute(10, "ontouchend", EventCallback.Factory.Create<TouchEventArgs>(this, OnTouchEnd));
            builder.AddEventStopPropagationAttribute(11, "ontouchend", true);
            builder.AddEventPreventDefaultAttribute(12, "ontouchend", true);
            builder.OpenComponent(13, componentType);
            builder.AddAttribute(14, "Group", Group);
            builder.CloseComponent();
            builder.CloseElement();
        }

        private void OnMouseDown(MouseEventArgs e) => Diagram.OnMouseDown(Group, e.ToCore());

        private void OnMouseUp(MouseEventArgs e) => Diagram.OnMouseUp(Group, e.ToCore());

        private void OnTouchStart(TouchEventArgs e) => Diagram.OnTouchStart(Group, e.ToCore());

        private void OnTouchEnd(TouchEventArgs e) => Diagram.OnTouchEnd(Group, e.ToCore());
    }
}
