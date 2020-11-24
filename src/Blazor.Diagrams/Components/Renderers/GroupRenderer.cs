using Blazor.Diagrams.Core;
using Blazor.Diagrams.Core.Extensions;
using Blazor.Diagrams.Core.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Linq;

namespace Blazor.Diagrams.Components.Renderers
{
    public class GroupRenderer : ComponentBase, IDisposable
    {
        [CascadingParameter(Name = "DiagramManager")]
        public DiagramManager DiagramManager { get; set; }

        [Parameter]
        public GroupModel Group { get; set; }

        public void Dispose()
        {
            
        }

        protected override void OnParametersSet()
        {
            Group.Changed += Group_Changed;
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            var position = Group.Position.Add(-20);
            var width = Group.Size.Width + 20 * 2;
            var height = Group.Size.Height + 20 * 2;

            builder.OpenElement(0, "div");
            builder.AddAttribute(1, "class", $"group {(Group.Selected ? "selected" : "")}");
            builder.AddAttribute(2, "style", $"position: absolute; top: {position.Y.ToInvariantString()}px; left: {position.X.ToInvariantString()}px; " +
                $"width: {width.ToInvariantString()}px; height: {height.ToInvariantString()}px; border: 1px solid black; position: absolute; user-select: none; cursor: move; pointer-events: all;");
            builder.AddAttribute(3, "onmousedown", EventCallback.Factory.Create<MouseEventArgs>(this, OnMouseDown));
            builder.AddEventStopPropagationAttribute(4, "onmousedown", true);
            builder.AddAttribute(5, "onmouseup", EventCallback.Factory.Create<MouseEventArgs>(this, OnMouseUp));
            builder.AddEventStopPropagationAttribute(6, "onmouseup", true);

            builder.OpenElement(7, "svg");
            builder.AddAttribute(8, "style", "position: absolute; width: 100%; height: 100%; overflow: visible; " +
                $"top: {(-position.Y).ToInvariantString()}px; left: {(-position.X).ToInvariantString()}px");
            
            foreach (var link in Group.Nodes.SelectMany(n => n.Ports.SelectMany(p => p.Links)).Distinct())
            {
                builder.OpenComponent<LinkRenderer>(9);
                builder.SetKey(link.Id);
                builder.AddAttribute(10, "Link", link);
                builder.CloseComponent();
            }

            builder.CloseElement();

            builder.OpenElement(11, "div");
            builder.AddAttribute(12, "style", "position: absolute; width: 100%; height: 100%; overflow: visible; " +
                $"top: {(-position.Y).ToInvariantString()}px; left: {(-position.X).ToInvariantString()}px");

            foreach (var node in Group.Nodes)
            {
                builder.OpenComponent<NodeRenderer>(12);
                builder.SetKey(node.Id);
                builder.AddAttribute(13, "Node", node);
                builder.CloseComponent();
            }

            builder.CloseElement();

            builder.CloseElement();
        }

        private void Group_Changed() => StateHasChanged();

        private void OnMouseDown(MouseEventArgs e) => DiagramManager.OnMouseDown(Group, e);

        private void OnMouseUp(MouseEventArgs e) => DiagramManager.OnMouseUp(Group, e);
    }
}
