using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Blazor.Diagrams.Core.Tests.Nodes
{
    public class NodeModelTests
    {
        public NodeModelTestFixture Fixture;
        public NodeModelTests()
        {
            Fixture = new NodeModelTestFixture();
        }

        [Fact]
        public void AddPort()
        {
            //Act
            //Arrange
            Fixture.Node.AddPort();
            //Assert
            Assert.Single(Fixture.Node.Ports);
        }

        [Fact]
        public void GetPort()
        {
            //Act
            //Arrange
            Fixture.Node.AddPort();
            var port=Fixture.Node.GetPort(Core.Models.PortAlignment.Bottom);
            //Assert
            Assert.NotNull(port);
        }

        [Fact]
        public void SetPosition()
        {
            //Act
            //Arrange
            Fixture.Node.SetPosition(100,200);
            //Assert
            Assert.Equal(100,Fixture.Node.Position.X);
            Assert.Equal(200,Fixture.Node.Position.Y);
        }

        [Fact]
        public void ReinitializePorts()
        {

            //Arrange
            Fixture.Node.AddPort();
            Fixture.Node.AddPort(Core.Models.PortAlignment.BottomLeft);
            //Act
            Fixture.Node.ReinitializePorts();
            //Assert
            Assert.All(Fixture.Node.Ports, y => Assert.False(y.Initialized));
        }
    }
}
