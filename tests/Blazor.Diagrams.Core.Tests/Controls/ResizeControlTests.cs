using Blazor.Diagrams.Core.Controls.Default;
using Blazor.Diagrams.Core.Events;
using Blazor.Diagrams.Core.Models.Base;
using Blazor.Diagrams.Core.Positions.Resizing;
using Moq;
using Xunit;

namespace Blazor.Diagrams.Core.Tests.Controls
{
    public class ResizeControlTests
    {
        [Fact]
        public void GetPosition_ShouldUseResizeProviderGetPosition()
        {
            var resizeProvider = new Mock<IResizerProvider>();
            var control = new ResizeControl(resizeProvider.Object);
            var model = new Mock<Model>();

            control.GetPosition(model.Object);

            resizeProvider.Verify(m => m.GetPosition(model.Object), Times.Once);
        }

        [Fact]
        public void OnPointerDown_ShouldInvokeResizeStart()
        {
            var resizeProvider = new Mock<IResizerProvider>();
            var control = new ResizeControl(resizeProvider.Object);
            var diagram = Mock.Of<Diagram>();
            var model = Mock.Of<Model>();
            var eventArgs = new PointerEventArgs(0, 0, 0, 0, false, false, false, 0, 0, 0, 0, 0, 0, string.Empty, true);

            control.OnPointerDown(diagram, model, eventArgs);

            resizeProvider.Verify(m => m.OnResizeStart(diagram, model, eventArgs), Times.Once);
        }

        [Fact]
        public void OnPointerDown_ShouldAddEventHandlers()
        {
            var resizeProvider = new Mock<IResizerProvider>();
            var control = new ResizeControl(resizeProvider.Object);
            var diagram = new TestDiagram();
            var model = Mock.Of<Model>();
            var eventArgs = new PointerEventArgs(0, 0, 0, 0, false, false, false, 0, 0, 0, 0, 0, 0, string.Empty, true);

            control.OnPointerDown(diagram, model, eventArgs);
            
            diagram.TriggerPointerMove(model, eventArgs);
            resizeProvider.Verify(m => m.OnPointerMove(model, eventArgs), Times.Once);

            diagram.TriggerPointerUp(model, eventArgs);
            resizeProvider.Verify(m => m.OnResizeEnd(model, eventArgs), Times.Once);
        }

        [Fact]
        public void OnPointerUp_ShouldRemoveEventHandlers()
        {
            var resizeProvider = new Mock<IResizerProvider>();
            var control = new ResizeControl(resizeProvider.Object);
            var diagram = new TestDiagram();
            var model = Mock.Of<Model>();
            var eventArgs = new PointerEventArgs(0, 0, 0, 0, false, false, false, 0, 0, 0, 0, 0, 0, string.Empty, true);

            control.OnPointerDown(diagram, model, eventArgs);
            diagram.TriggerPointerUp(model, eventArgs);

            diagram.TriggerPointerMove(model, eventArgs);
            resizeProvider.Verify(m => m.OnPointerMove(model, eventArgs), Times.Never);

            diagram.TriggerPointerUp(model, eventArgs);
            resizeProvider.Verify(m => m.OnResizeEnd(model, eventArgs), Times.Once);
        }
    }
}
