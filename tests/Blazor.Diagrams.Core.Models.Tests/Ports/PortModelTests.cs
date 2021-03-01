using Xunit;

namespace Blazor.Diagrams.Core.Models.Tests.Ports
{
    public class PortModelTests
    {
        readonly PortModelFixture Fixture;
        public PortModelTests()
        {
            Fixture = new PortModelFixture();
        }
        [Fact]
        public void CannotAttachToSelf()
        {
            //act
            var result = Fixture.PortModel.CanAttachTo(Fixture.PortModel);
            //Assert
            Assert.False(result);
        }

        [Fact]
        public void CannotAttachToLockedPort()
        {
            //Arrange
            var target = Fixture.CreatePortModel();
            target.Locked = true;
            //act
            var result = Fixture.PortModel.CanAttachTo(target);
            //Assert
            Assert.False(result);
        }
        [Fact]
        public void CannotAttachToPortWithSameParent()
        {
            //Arrange
            var target = Fixture.PortModelParent.GetPort(PortAlignment.Top);
            //act
            var result = Fixture.PortModel.CanAttachTo(target);
            //Assert
            Assert.False(result);
        }

        [Fact]
        public void GetParent()
        {
            //act
            var result = Fixture.PortModel.GetParent<NodeModel>();
            //Assert
            Assert.Equal(Fixture.PortModelParent, result);
        }

    }
}
