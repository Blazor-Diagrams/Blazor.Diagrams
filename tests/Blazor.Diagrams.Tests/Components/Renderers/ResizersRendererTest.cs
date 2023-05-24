using Blazor.Diagrams.Components.Renderers;
using Blazor.Diagrams.Core.Events;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Bunit;
using FluentAssertions;
using Xunit;

namespace Blazor.Diagrams.Tests.Components.Renderers
{
    public class ResizersRendererTest
    {
        [Fact]
        public void ShouldRenderResizer()
        {
            using var ctx = new TestContext();
            var node = new NodeModel();
            var resizer = new ResizerModel(node, ResizerPosition.TopLeft);

            var component = ctx.RenderComponent<ResizerRenderer>(parameters => parameters
                .Add(n => n.Resizer, resizer)
                .Add(n => n.ResizerClass, "my-resizer")
                .Add(n => n.BlazorDiagram, new BlazorDiagram()));

            component.MarkupMatches("<div class=\"topleft my-resizer\"></div>");
        }
    }
}
