using Blazor.Diagrams.Components.Renderers;
using Blazor.Diagrams.Components;
using Blazor.Diagrams.Core.Models;
using Bunit;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Blazor.Diagrams.Components.Widgets;
using AngleSharp.Dom;
using AngleSharp.Css.Dom;
using Blazor.Diagrams.Options;
using Blazor.Diagrams.Core.Options;
using Blazor.Diagrams.Core;
using Blazor.Diagrams.Core.Events;
using Blazor.Diagrams.Core.Geometry;

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
            diagram.Options.Behaviors.DiagramDragBehavior = DiagramDragBehavior.Select;
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
            widget.GetStyle().GetWidth().Equals(100);
            widget.GetStyle().GetHeight().Equals(150);
            widget.GetStyle().GetTop().Equals(175);
            widget.GetStyle().GetLeft().Equals(300);
        }
    }
}
