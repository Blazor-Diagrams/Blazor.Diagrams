using AngleSharp.Css.Dom;
using Blazor.Diagrams.Components;
using Blazor.Diagrams.Core.Geometry;
using Bunit;
using FluentAssertions;
using Xunit;

namespace Blazor.Diagrams.Core.Tests.Behaviors
{
    public class DiagramCursorTests
    {
        [Fact]
        public void Behavior_WhenPanningOptionIsAllowed_CursorShouldBeGrab()
        {
            // Arrange
            using var ctx = new TestContext();
            var diagram = new BlazorDiagram();
            diagram.Options.AllowPanning = true;
            ctx.JSInterop.Setup<Rectangle>("ZBlazorDiagrams.getBoundingClientRect", _ => true);

            // Act
            var cut = ctx.RenderComponent<DiagramCanvas>(parameters => parameters
            .Add(n => n.BlazorDiagram, diagram));
            var diagramCanvas = cut.Find(".diagram-canvas");
            var canvasStyle = diagramCanvas.GetStyle().CssText;

            // Assert
            canvasStyle.Should().Contain("cursor: grab");
        }

        [Fact]
        public void Behavior_WhenPanningOptionIsNotAllowed_CursorShouldBeDefault()
        {
            // Arrange
            using var ctx = new TestContext();
            var diagram = new BlazorDiagram();
            diagram.Options.AllowPanning = false;
            ctx.JSInterop.Setup<Rectangle>("ZBlazorDiagrams.getBoundingClientRect", _ => true);

            // Act
            var cut = ctx.RenderComponent<DiagramCanvas>(parameters => parameters
            .Add(n => n.BlazorDiagram, diagram));
            var diagramCanvas = cut.Find(".diagram-canvas");
            var canvasStyle = diagramCanvas.GetStyle().CssText;

            // Assert
            canvasStyle.Should().Contain("cursor: default");
        }
    }
}
