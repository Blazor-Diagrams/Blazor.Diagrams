using Blazor.Diagrams.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Blazor.Diagrams.Core.Models.Tests.Links
{
    public class LinkModelTests
    {
        readonly LinkModelTestFixture Fixture;

        public LinkModelTests()
        {
            Fixture = new LinkModelTestFixture();
        }
        [Fact]
        public void SetSourcePort()
        {
            //Arrange
            var link = new LinkModel(Fixture.SourceNode.GetPort(PortAlignment.Bottom));

            //Act
            link.SetSourcePort(Fixture.AnotherNode.GetPort(PortAlignment.Bottom));

            //Assert
            Assert.Equal(Fixture.AnotherNode.GetPort(PortAlignment.Bottom).Id, link.SourcePort.Id);
            Assert.Equal(Fixture.AnotherNode.Id, link.SourcePort.Parent.Id);
        }

        [Fact]
        public void SetTargetPort()
        {
            //Arrange
            var link = Fixture.CreateLink();

            //Act
            link.SetTargetPort(Fixture.AnotherNode.GetPort(PortAlignment.Bottom));

            //Assert
            Assert.Equal(Fixture.AnotherNode.GetPort(PortAlignment.Bottom).Id,link.TargetPort.Id);
            Assert.Equal(Fixture.AnotherNode.Id, link.TargetPort.Parent.Id);
        }
    }
}
