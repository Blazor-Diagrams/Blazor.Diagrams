using Blazor.Diagrams.Core.Behaviors;
using Blazor.Diagrams.Core.Events;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace Blazor.Diagrams.Core.Tests.Behaviors;

public class EventsBehaviorTests
{
    [Fact]
    public void Behavior_ShouldNotTriggerMouseClick_WhenItsRemoved()
    {
        // Arrange
        var diagram = new TestDiagram();
        diagram.UnregisterBehavior<EventsBehavior>();
        var eventTriggered = false;

        // Act
        diagram.PointerClick += (m, e) => eventTriggered = true;
        diagram.TriggerPointerDown(null, new PointerEventArgs(0, 0, 0, 0, false, false, false, 0, 0, 0, 0, 0, 0, string.Empty, true));
        diagram.TriggerPointerUp(null, new PointerEventArgs(0, 0, 0, 0, false, false, false, 0, 0, 0, 0, 0, 0, string.Empty, true));

        // Assert
        eventTriggered.Should().BeFalse();
    }

    [Fact]
    public void Behavior_ShouldTriggerMouseClick_WhenMouseDownThenUpWithoutMove()
    {
        // Arrange
        var diagram = new TestDiagram();
        var eventTriggered = false;

        // Act
        diagram.PointerClick += (m, e) => eventTriggered = true;
        diagram.TriggerPointerDown(null, new PointerEventArgs(0, 0, 0, 0, false, false, false, 0, 0, 0, 0, 0, 0, string.Empty, true));
        diagram.TriggerPointerUp(null, new PointerEventArgs(0, 0, 0, 0, false, false, false, 0, 0, 0, 0, 0, 0, string.Empty, true));

        // Assert
        eventTriggered.Should().BeTrue();
    }

    [Fact]
    public void Behavior_ShouldNotTriggerMouseClick_WhenMouseMoves()
    {
        // Arrange
        var diagram = new TestDiagram();
        var eventTriggered = false;

        // Act
        diagram.PointerClick += (m, e) => eventTriggered = true;
        diagram.TriggerPointerDown(null, new PointerEventArgs(0, 0, 0, 0, false, false, false, 0, 0, 0, 0, 0, 0, string.Empty, true));
        diagram.TriggerPointerMove(null, new PointerEventArgs(0, 0, 0, 0, false, false, false, 0, 0, 0, 0, 0, 0, string.Empty, true));
        diagram.TriggerPointerUp(null, new PointerEventArgs(0, 0, 0, 0, false, false, false, 0, 0, 0, 0, 0, 0, string.Empty, true));

        // Assert
        eventTriggered.Should().BeFalse();
    }

    [Fact]
    public void Behavior_ShouldTriggerMouseDoubleClick_WhenTwoMouseClicksHappenWithinTime()
    {
        // Arrange
        var diagram = new TestDiagram();
        var eventTriggered = false;

        // Act
        diagram.PointerDoubleClick += (m, e) => eventTriggered = true;
        diagram.TriggerPointerClick(null, new PointerEventArgs(0, 0, 0, 0, false, false, false, 0, 0, 0, 0, 0, 0, string.Empty, true));
        diagram.TriggerPointerClick(null, new PointerEventArgs(0, 0, 0, 0, false, false, false, 0, 0, 0, 0, 0, 0, string.Empty, true));

        // Assert
        eventTriggered.Should().BeTrue();
    }

    [Fact]
    public async Task Behavior_ShouldNotTriggerMouseDoubleClick_WhenTimeExceeds500()
    {
        // Arrange
        var diagram = new TestDiagram();
        var eventTriggered = false;

        // Act
        diagram.PointerDoubleClick += (m, e) => eventTriggered = true;
        diagram.TriggerPointerClick(null, new PointerEventArgs(0, 0, 0, 0, false, false, false, 0, 0, 0, 0, 0, 0, string.Empty, true));
        await Task.Delay(520);
        diagram.TriggerPointerClick(null, new PointerEventArgs(0, 0, 0, 0, false, false, false, 0, 0, 0, 0, 0, 0, string.Empty, true));

        // Assert
        eventTriggered.Should().BeFalse();
    }

    [Fact]
    public void Behavior_ShouldTriggerMouseClick_OnlyWhenMouseDownWasAlsoTriggered_Issue204()
    {
        // Arrange
        var diagram = new TestDiagram();
        var eventTriggered = false;

        // Act
        diagram.PointerClick += (m, e) => eventTriggered = true;
        diagram.TriggerPointerUp(null, new PointerEventArgs(0, 0, 0, 0, false, false, false, 0, 0, 0, 0, 0, 0, string.Empty, true));

        // Assert
        eventTriggered.Should().BeFalse();
    }
}
