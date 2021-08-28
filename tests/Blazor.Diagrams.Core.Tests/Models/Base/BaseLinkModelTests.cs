using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;
using FluentAssertions;
using Xunit;

namespace Blazor.Diagrams.Core.Tests.Models.Base
{
    public class BaseLinkModelTests
    {
        [Fact]
        public void SetSourcePort_ShouldChangePropertiesAndTriggerEvent()
        {
            // Arrange
            var link = new TestLink(sourcePort: new PortModel(null), targetPort: null);
            var parent = new NodeModel();
            var sp = new PortModel(parent);
            var eventsTriggered = 0;
            PortModel oldSp = null;
            PortModel newSp = null;
            BaseLinkModel linkInstance = null;

            // Act
            link.SourcePortChanged += (l, o, n) =>
            {
                eventsTriggered++;
                linkInstance = l;
                oldSp = o;
                newSp = n;
            };

            link.SetSourcePort(sp);

            // Assert
            eventsTriggered.Should().Be(1);
            link.SourcePort.Should().BeSameAs(sp);
            oldSp.Should().NotBeNull();
            newSp.Should().BeSameAs(sp);
            linkInstance.Should().BeSameAs(link);
            link.SourceNode.Should().BeSameAs(parent);
        }

        [Fact]
        public void SetTargetPort_ShouldChangePropertiesAndTriggerEvent()
        {
            // Arrange
            var link = new TestLink(sourcePort: new PortModel(null), targetPort: null);
            var parent = new NodeModel();
            var tp = new PortModel(parent);
            var eventsTriggered = 0;
            PortModel oldTp = null;
            PortModel newTp = null;
            BaseLinkModel linkInstance = null;

            // Act
            link.TargetPortChanged += (l, o, n) =>
            {
                eventsTriggered++;
                linkInstance = l;
                oldTp = o;
                newTp = n;
            };

            link.SetTargetPort(tp);

            // Assert
            eventsTriggered.Should().Be(1);
            link.TargetPort.Should().BeSameAs(tp);
            oldTp.Should().BeNull();
            newTp.Should().BeSameAs(tp);
            linkInstance.Should().BeSameAs(link);
            link.TargetNode.Should().BeSameAs(parent);
        }

        private class TestLink : BaseLinkModel
        {
            public TestLink(NodeModel sourceNode, NodeModel targetNode) : base(sourceNode, targetNode)
            {
            }

            public TestLink(PortModel sourcePort, PortModel targetPort = null) : base(sourcePort, targetPort)
            {
            }

            public TestLink(string id, NodeModel sourceNode, NodeModel targetNode) : base(id, sourceNode, targetNode)
            {
            }

            public TestLink(string id, PortModel sourcePort, PortModel targetPort = null) : base(id, sourcePort, targetPort)
            {
            }
        }
    }
}
