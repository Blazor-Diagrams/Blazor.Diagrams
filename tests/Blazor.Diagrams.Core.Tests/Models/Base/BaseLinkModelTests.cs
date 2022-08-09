using Blazor.Diagrams.Core.Anchors;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;
using FluentAssertions;
using Xunit;

namespace Blazor.Diagrams.Core.Tests.Models.Base
{
    public class BaseLinkModelTests
    {
        [Fact]
        public void SetSource_ShouldChangePropertiesAndTriggerEvent()
        {
            // Arrange
            var link = new LinkModel(sourcePort: new PortModel(null), targetPort: null);
            var parent = new NodeModel();
            var sp = new SinglePortAnchor(new PortModel(parent));
            var eventsTriggered = 0;
            Anchor? oldSp = null;
            Anchor? newSp = null;
            BaseLinkModel? linkInstance = null;

            // Act
            link.SourceChanged += (l, o, n) =>
            {
                eventsTriggered++;
                linkInstance = l;
                oldSp = o;
                newSp = n;
            };

            link.SetSource(sp);

            // Assert
            eventsTriggered.Should().Be(1);
            link.Source.Should().BeSameAs(sp);
            oldSp.Should().NotBeNull();
            newSp.Should().BeSameAs(sp);
            linkInstance.Should().BeSameAs(link);
            link.Source.Node.Should().BeSameAs(parent);
        }

        [Fact]
        public void SetTarget_ShouldChangePropertiesAndTriggerEvent()
        {
            // Arrange
            var link = new LinkModel(sourcePort: new PortModel(null), targetPort: null);
            var parent = new NodeModel();
            var tp = new SinglePortAnchor(new PortModel(parent));
            var eventsTriggered = 0;
            Anchor? oldTp = null;
            Anchor? newTp = null;
            BaseLinkModel? linkInstance = null;

            // Act
            link.TargetChanged += (l, o, n) =>
            {
                eventsTriggered++;
                linkInstance = l;
                oldTp = o;
                newTp = n;
            };

            link.SetTarget(tp);

            // Assert
            eventsTriggered.Should().Be(1);
            link.Target.Should().BeSameAs(tp);
            oldTp.Should().BeNull();
            newTp.Should().BeSameAs(tp);
            linkInstance.Should().BeSameAs(link);
            link.Target!.Node.Should().BeSameAs(parent);
        }
    }
}
