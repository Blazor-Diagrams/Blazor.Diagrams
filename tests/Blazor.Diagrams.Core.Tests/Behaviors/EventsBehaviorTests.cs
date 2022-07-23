using Blazor.Diagrams.Core.Behaviors;
using Blazor.Diagrams.Core.Events;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace Blazor.Diagrams.Core.Tests.Behaviors
{
    public class EventsBehaviorTests
    {
        [Fact]
        public void Behavior_ShouldNotTriggerMouseClick_WhenItsRemoved()
        {
            // Arrange
            var diagram = new DiagramBase();
            diagram.UnregisterBehavior<EventsBehavior>();
            var eventTriggered = false;

            // Act
            diagram.MouseClick += (m, e) => eventTriggered = true;
            diagram.OnMouseDown(null, new MouseEventArgs(0, 0, 0, 0, false, false, false));
            diagram.OnMouseUp(null, new MouseEventArgs(0, 0, 0, 0, false, false, false));

            // Assert
            eventTriggered.Should().BeFalse();
        }

        [Fact]
        public void Behavior_ShouldTriggerMouseClick_WhenMouseDownThenUpWithoutMove()
        {
            // Arrange
            var diagram = new DiagramBase();
            var eventTriggered = false;

            // Act
            diagram.MouseClick += (m, e) => eventTriggered = true;
            diagram.OnMouseDown(null, new MouseEventArgs(0, 0, 0, 0, false, false, false));
            diagram.OnMouseUp(null, new MouseEventArgs(0, 0, 0, 0, false, false, false));

            // Assert
            eventTriggered.Should().BeTrue();
        }

        [Fact]
        public void Behavior_ShouldNotTriggerMouseClick_WhenMouseMoves()
        {
            // Arrange
            var diagram = new DiagramBase();
            var eventTriggered = false;

            // Act
            diagram.MouseClick += (m, e) => eventTriggered = true;
            diagram.OnMouseDown(null, new MouseEventArgs(0, 0, 0, 0, false, false, false));
            diagram.OnMouseMove(null, new MouseEventArgs(0, 0, 0, 0, false, false, false));
            diagram.OnMouseUp(null, new MouseEventArgs(0, 0, 0, 0, false, false, false));

            // Assert
            eventTriggered.Should().BeFalse();
        }

        [Fact]
        public void Behavior_ShouldTriggerMouseDoubleClick_WhenTwoMouseClicksHappenWithinTime()
        {
            // Arrange
            var diagram = new DiagramBase();
            var eventTriggered = false;

            // Act
            diagram.MouseDoubleClick += (m, e) => eventTriggered = true;
            diagram.OnMouseClick(null, new MouseEventArgs(0, 0, 0, 0, false, false, false));
            diagram.OnMouseClick(null, new MouseEventArgs(0, 0, 0, 0, false, false, false));

            // Assert
            eventTriggered.Should().BeTrue();
        }

        [Fact]
        public async Task Behavior_ShouldNotTriggerMouseDoubleClick_WhenTimeExceeds500()
        {
            // Arrange
            var diagram = new DiagramBase();
            var eventTriggered = false;

            // Act
            diagram.MouseDoubleClick += (m, e) => eventTriggered = true;
            diagram.OnMouseClick(null, new MouseEventArgs(0, 0, 0, 0, false, false, false));
            await Task.Delay(520);
            diagram.OnMouseClick(null, new MouseEventArgs(0, 0, 0, 0, false, false, false));

            // Assert
            eventTriggered.Should().BeFalse();
        }

        [Fact]
        public void Behavior_ShouldTriggerMouseClick_OnlyWhenMouseDownWasAlsoTriggered_Issue204()
        {
            // Arrange
            var diagram = new DiagramBase();
            var eventTriggered = false;

            // Act
            diagram.MouseClick += (m, e) => eventTriggered = true;
            diagram.OnMouseUp(null, new MouseEventArgs(0, 0, 0, 0, false, false, false));

            // Assert
            eventTriggered.Should().BeFalse();
        }
    }
}
