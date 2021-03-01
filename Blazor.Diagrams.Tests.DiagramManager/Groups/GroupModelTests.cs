using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Blazor.Diagrams.Core.Tests.Groups
{
    public class GroupModelTests
    {
        GroupModelFixture Fixture;
        public GroupModelTests()
        {
            Fixture = new GroupModelFixture();
        }

       
        [Fact]
        public void AddPort()
        {
            //arrange

            //act
            Fixture.GroupModel.AddPort();
            //Assert
            Assert.Single(Fixture.GroupModel.Ports);
        }

        [Fact]
        public void AddPorts()
        {
            //act
            Fixture.GroupModel.AddPort();
            Fixture.GroupModel.AddPort();
            //Assert
            Assert.Equal(2,Fixture.GroupModel.Ports.Count);
        }

        [Fact]
        public void Ungroup()
        {
            //act
            Fixture.GroupModel.Ungroup();
            //Assert
            Assert.All(Fixture.GroupModel.Children, x => Assert.Null(x.Group));
            Assert.All(Fixture.Nodes, x => Assert.Null(x.Group));
           
        }

        [Fact]
        public void ReinitializePorts()
        {
            //act
            Fixture.GroupModel.ReinitializePorts();
            //Assert
            Assert.All(Fixture.GroupModel.Ports, x => Assert.True(x.Initialized));
        }

        [Fact]
        public void SetPosition()
        {
            //act
            Fixture.GroupModel.SetPosition(100, 200);
            //Assert
            Assert.Equal(100, Fixture.GroupModel.Position.X);
            Assert.Equal(200, Fixture.GroupModel.Position.Y);
        }

        
    }
}
