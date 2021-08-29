using Blazor.Diagrams.Core.Behaviors;
using FluentAssertions;
using Microsoft.AspNetCore.Components.Web;
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
            var diagram = new Diagram();
            diagram.UnregisterBehavior<EventsBehavior>();
            var eventTriggered = false;

            // Act
            diagram.MouseClick += (m, e) => eventTriggered = true;
            diagram.OnMouseDown(null, new MouseEventArgs());
            diagram.OnMouseUp(null, new MouseEventArgs());

            // Assert
            eventTriggered.Should().BeFalse();
        }

        [Fact]
        public void Behavior_ShouldTriggerMouseClick_WhenMouseDownThenUpWithoutMove()
        {
            // Arrange
            var diagram = new Diagram();
            var eventTriggered = false;

            // Act
            diagram.MouseClick += (m, e) => eventTriggered = true;
            diagram.OnMouseDown(null, new MouseEventArgs());
            diagram.OnMouseUp(null, new MouseEventArgs());

            // Assert
            eventTriggered.Should().BeTrue();
        }

        [Fact]
        public void Behavior_ShouldNotTriggerMouseClick_WhenMouseMoves()
        {
            // Arrange
            var diagram = new Diagram();
            var eventTriggered = false;

            // Act
            diagram.MouseClick += (m, e) => eventTriggered = true;
            diagram.OnMouseDown(null, new MouseEventArgs());
            diagram.OnMouseMove(null, new MouseEventArgs());
            diagram.OnMouseUp(null, new MouseEventArgs());

            // Assert
            eventTriggered.Should().BeFalse();
        }

        [Fact]
        public void Behavior_ShouldTriggerMouseDoubleClick_WhenTwoMouseClicksHappenWithinTime()
        {
            // Arrange
            var diagram = new Diagram();
            var eventTriggered = false;

            // Act
            diagram.MouseDoubleClick += (m, e) => eventTriggered = true;
            diagram.OnMouseClick(null, new MouseEventArgs());
            diagram.OnMouseClick(null, new MouseEventArgs());

            // Assert
            eventTriggered.Should().BeTrue();
        }

        [Fact]
        public async Task Behavior_ShouldNotTriggerMouseDoubleClick_WhenTimeExceeds500()
        {
            // Arrange
            var diagram = new Diagram();
            var eventTriggered = false;

            // Act
            diagram.MouseDoubleClick += (m, e) => eventTriggered = true;
            diagram.OnMouseClick(null, new MouseEventArgs());
            await Task.Delay(520);
            diagram.OnMouseClick(null, new MouseEventArgs());

            // Assert
            eventTriggered.Should().BeFalse();
        }
    }
}
