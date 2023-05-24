using Bunit;
using Xunit;
using Blazor.Diagrams.Components.Widgets;
using AngleSharp.Css.Dom;
using Blazor.Diagrams.Core.Events;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Behaviors;

namespace Blazor.Diagrams.Tests.Components.Widgets
{
    public class SelectionBoxWidgetTests
    {
        [Fact]
        public void SelectionBoxWidget_SelectionBoundsChanged_RendersSelectionBoxWidgetInCorrectLocation()
        {
            // Arrange
            using var ctx = new TestContext();
            var diagram = new BlazorDiagram();
            diagram.BehaviorOptions.DiagramDragBehavior = diagram.GetBehavior<SelectionBoxBehavior>();
            diagram.SetPan(-75, -100);
            diagram.SetContainer(new Rectangle(new Point(0, 0), new Size(500, 500)));

            // Act
            var cut = ctx.RenderComponent<SelectionBoxWidget>(parameters => parameters
                .Add(n => n.BlazorDiagram, diagram));
            Assert.Throws<ElementNotFoundException>(() => cut.Find("div"));
            diagram.TriggerPointerDown(null,
                new PointerEventArgs(100, 150, 0, 0, false, false, false, 0, 0, 0, 0, 0, 0, string.Empty, true));
            diagram.TriggerPointerMove(null,
                new PointerEventArgs(200, 250, 0, 0, false, false, false, 0, 0, 0, 0, 0, 0, string.Empty, true));


            // Assert
            var widget = cut.Find("div");
            Assert.Equal("100px", widget.GetStyle().GetWidth());
            Assert.Equal("100px", widget.GetStyle().GetHeight());
            Assert.Equal("150px", widget.GetStyle().GetTop());
            Assert.Equal("100px", widget.GetStyle().GetLeft());
        }
    }
}
