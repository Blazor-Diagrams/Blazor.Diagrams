using Blazor.Diagrams.Core;
using Blazor.Diagrams.Core.Extensions;
using Blazor.Diagrams.Core.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;

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
            throw new NotImplementedException();
        }

        protected override void OnParametersSet()
        {
            Group.Changed += Group_Changed;
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            builder.OpenElement(0, "div");
            builder.AddAttribute(1, "class", "group");

            var position = Group.Position.Add(20 * -1);
            var width = Group.Size.Width + 20 * 2;
            var height = Group.Size.Height + 20 * 2;
            builder.AddAttribute(2, "style", $"position: absolute; top: {position.Y.ToInvariantString()}px; left: {position.X.ToInvariantString()}px; " +
                $"width: {width.ToInvariantString()}px; height: {height.ToInvariantString()}px; border: 1px solid black;");

            foreach (var node in Group.Nodes)
            {
                builder.OpenComponent<NodeRenderer>(2);
                builder.AddAttribute(3, "Node", node);
                builder.CloseComponent();
            }

            builder.CloseElement();
        }

        private void Group_Changed() => StateHasChanged();
    }
}
