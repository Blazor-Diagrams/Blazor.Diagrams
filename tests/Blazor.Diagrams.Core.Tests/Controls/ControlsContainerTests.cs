using Blazor.Diagrams.Core.Controls;
using Blazor.Diagrams.Core.Events;
using Blazor.Diagrams.Core.Models;
using Xunit;

namespace Blazor.Diagrams.Core.Tests.Controls
{
    public class ControlsContainerTests
    {
        [Fact]
        public void AlwaysOnControlType_AlwaysVisible()
        {
            // Arrange
            var diagram = new TestDiagram();
            var node = diagram.Nodes.Add(new NodeModel());
            var controls = diagram.Controls.AddFor(node, ControlsType.AlwaysOn);

            // Assert
            Assert.True(controls.Visible);

            node.Selected = true;
            Assert.True(controls.Visible);

            diagram.TriggerPointerEnter(node, new PointerEventArgs(0, 0, 0, 0, false, false, false, 0, 0, 0, 0, 0, 0, string.Empty, true));
            Assert.True(controls.Visible);
        }
    }
}
