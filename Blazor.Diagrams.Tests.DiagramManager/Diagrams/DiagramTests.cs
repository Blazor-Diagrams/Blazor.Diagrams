using Blazor.Diagrams.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Blazor.Diagrams.Core.Tests.Diagrams
{
    public class DiagramTests
    {
        DiagramTestFixture Fixture;
        public DiagramTests()
        {
            Fixture = new DiagramTestFixture();
        }
        [Fact]
        public void AddNode()
        {
            //Arrange
            NodeModel node = new NodeModel();
            //Act
            Fixture.Diagram.Nodes.Add(node);
            //Assert
            Assert.Single(Fixture.Diagram.Nodes);
        }

        [Fact]
        public void AddNodes()
        {
            //Arrange
            NodeModel node1 = new NodeModel();
            NodeModel node2 = new NodeModel();
            //Act
            Fixture.Diagram.Nodes.Add(node1, node2);
            //Assert
            Assert.Equal(2, Fixture.Diagram.Nodes.Count);
        }
        [Fact]
        public void AddNodesAsArray()
        {
            //Arrange
            List<NodeModel> nodes = new List<NodeModel>()
            {
                new NodeModel(),
                new NodeModel()
            };
            //Act
            Fixture.Diagram.Nodes.Add(nodes.ToArray());
            //Assert
            Assert.Equal(2, Fixture.Diagram.Nodes.Count);
        }

        [Fact]
        public void RemoveNode()
        {
            //Arrange
            var node = new NodeModel();
            Fixture.Diagram.Nodes.Add(node);
            //Act
            Fixture.Diagram.Nodes.Remove(node);
            //Assert
            Assert.Empty(Fixture.Diagram.Nodes);
        }


        [Fact]
        public void RemoveNodes()
        {
            //Arrange
            List<NodeModel> nodes = new List<NodeModel>()
            {
                new NodeModel(),
                new NodeModel()
            };
            Fixture.Diagram.Nodes.Add(nodes.ToArray());
            //Act
            Fixture.Diagram.Nodes.Remove(nodes.ToArray());
            //Assert
            Assert.Empty(Fixture.Diagram.Nodes);
        }

        [Fact]
        public void AddLink()
        {
            //Arrange
            NodeModel sourceNode = new NodeModel();
            sourceNode.AddPort();
            NodeModel destinationNode = new NodeModel();
            LinkModel linkModel = new LinkModel(sourceNode.GetPort(PortAlignment.Bottom));
            //Act
            Fixture.Diagram.Nodes.Add(sourceNode);
            Fixture.Diagram.Nodes.Add(destinationNode);
            Fixture.Diagram.Links.Add(linkModel);
            //Assert
            Assert.Single(Fixture.Diagram.Links);
        }
        [Fact]
        public void AddLinkWithSourcePortAndDestinationPort()
        {
            //Arrange
            NodeModel sourceNode = new NodeModel();
            sourceNode.AddPort();
            NodeModel destinationNode = new NodeModel();
            destinationNode.AddPort();
            LinkModel linkModel = new LinkModel(sourceNode.GetPort(PortAlignment.Bottom), destinationNode.GetPort(PortAlignment.Bottom));
            Fixture.Diagram.Nodes.Add(sourceNode);
            Fixture.Diagram.Nodes.Add(destinationNode);
            //Act
            Fixture.Diagram.Links.Add(linkModel);
            //Assert
            Assert.Single(Fixture.Diagram.Links);
        }

        [Fact]
        public void RemoveLink()
        {
            //Arrange
            NodeModel sourceNode = new NodeModel();
            sourceNode.AddPort();
            NodeModel destinationNode = new NodeModel();
            LinkModel linkModel = new LinkModel(sourceNode.GetPort(PortAlignment.Bottom));
            Fixture.Diagram.Nodes.Add(sourceNode);
            Fixture.Diagram.Nodes.Add(destinationNode);
            Fixture.Diagram.Links.Add(linkModel);
            //Act
            Fixture.Diagram.Links.Remove(linkModel);
            //Assert
            Assert.Empty(Fixture.Diagram.Links);
        }

        [Fact]
        public void AddGroup()
        {
            //Arrange
            GroupModel group = new GroupModel(Enumerable.Empty<NodeModel>().ToArray());
            //Act
            Fixture.Diagram.AddGroup(group);
            //Assert
            Assert.Single(Fixture.Diagram.Groups);
        }
        [Fact]
        public void GroupNodes()
        {
            //Arrange
            NodeModel node1 = new NodeModel();
            NodeModel node2 = new NodeModel();
            Fixture.Diagram.Nodes.Add(node1, node2);
            //Act
            Fixture.Diagram.Group(node1, node2);
            var group = Fixture.Diagram.Groups.FirstOrDefault();
            //Assert
            Assert.Single(Fixture.Diagram.Groups);
            Assert.Equal(2, group.Children.Count());

        }

        [Fact]
        public void Ungroup()
        {
            //Arrange
            NodeModel node1 = new NodeModel();
            NodeModel node2 = new NodeModel();
            Fixture.Diagram.Nodes.Add(node1, node2);
            GroupModel groupModel = new GroupModel(new NodeModel[] { node1, node2 });
            Fixture.Diagram.AddGroup(groupModel);
            //Act
            Fixture.Diagram.Ungroup(groupModel);
            //Assert
            Assert.Equal(0,Fixture.Diagram.Groups.Count);

        }
        [Fact]
        public void RemoveGroup()
        {
            //Arrange
            NodeModel node1 = new NodeModel();
            NodeModel node2 = new NodeModel(); 
            Fixture.Diagram.Nodes.Add(node1, node2);
            var group=Fixture.Diagram.Group(node1,node2);
            //Act
            Fixture.Diagram.RemoveGroup(group);
            Assert.Equal(0, Fixture.Diagram.Groups.Count);

        }

        [Fact]
        public void GroupExceptionWhenNodeAlreadyHasGroup()
        {
            //Arrange
            NodeModel node1 = new NodeModel();
            Fixture.Diagram.Nodes.Add(node1);
            //act
            Fixture.Diagram.Group(node1);
            //Assert
            Assert.Throws<InvalidOperationException>(() => Fixture.Diagram.Group(node1));
        }
        [Fact]
        public void GroupExceptionWhenNodeNotInDiagram()
        {
            //Arrange
            NodeModel node1 = new NodeModel();
            //Assert
            Assert.Throws<Exception>(() => Fixture.Diagram.Group(node1));
        }

        [Fact]
        public void GroupExceptionWhenOneOfNodesNotInDiagram()
        {
            //Arrange
            NodeModel node1 = new NodeModel();
            NodeModel node2 = new NodeModel();
            Fixture.Diagram.Nodes.Add(node1);
            //Assert
            Assert.Throws<Exception>(() => Fixture.Diagram.Group(node1,node2));
        }


        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void SelectNodeModel(bool unselectAll)
        {
            //Arrange
            NodeModel node = new NodeModel();
            //Act
            Fixture.Diagram.Nodes.Add(node);
            Fixture.Diagram.SelectModel(node, unselectAll);
            //Assert
            Assert.Single(Fixture.Diagram.GetSelectedModels());
            Assert.True(node.Selected);
            Assert.IsType<NodeModel>(Fixture.Diagram.GetSelectedModels().First());
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void SelectSeveralNodes(bool unselectAll)
        {
            //Arrange
            NodeModel node1 = new NodeModel();
            node1.AddPort();
            NodeModel node2 = new NodeModel();
            node2.AddPort();
            NodeModel node3 = new NodeModel();
            Fixture.Diagram.Nodes.Add(node1);
            Fixture.Diagram.Nodes.Add(node2);
            Fixture.Diagram.Nodes.Add(node3);
            //Act

            Fixture.Diagram.SelectModel(node1, unselectAll);
            Fixture.Diagram.SelectModel(node2, unselectAll);
            //Assert
            if (unselectAll)
            {
                Assert.Single(Fixture.Diagram.GetSelectedModels());
            }
            else
            {
                Assert.Equal(2,Fixture.Diagram.GetSelectedModels().Count());
                Assert.True(node1.Selected);
                Assert.True(node2.Selected);
                Assert.False(node3.Selected);
            }
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void SelectLink(bool unselectAll)
        {
            //Arrange
            NodeModel node1 = new NodeModel();
            node1.AddPort();
            NodeModel node2 = new NodeModel();
            node2.AddPort();
            NodeModel node3 = new NodeModel();
            Fixture.Diagram.Nodes.Add(node1);
            Fixture.Diagram.Nodes.Add(node2);
            var link = new LinkModel(node1.GetPort(PortAlignment.Bottom), node2.GetPort(PortAlignment.Bottom));
            Fixture.Diagram.Links.Add(link);
            //Act

            Fixture.Diagram.SelectModel(link, unselectAll);
            //Assert

            Assert.Single(Fixture.Diagram.GetSelectedModels());
            Assert.IsType<LinkModel>(Fixture.Diagram.GetSelectedModels().First());
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void SelectSeveralLinks(bool unselectAll)
        {
            //Arrange
            NodeModel node1 = new NodeModel();
            node1.AddPort();
            NodeModel node2 = new NodeModel();
            node2.AddPort();
            NodeModel node3 = new NodeModel();
            node3.AddPort();
            Fixture.Diagram.Nodes.Add(node1);
            Fixture.Diagram.Nodes.Add(node2);
            var link1 = new LinkModel(node1.GetPort(PortAlignment.Bottom), node2.GetPort(PortAlignment.Bottom));
            var link2 = new LinkModel(node2.GetPort(PortAlignment.Bottom), node3.GetPort(PortAlignment.Bottom));
            Fixture.Diagram.Links.Add(link1);
            Fixture.Diagram.Links.Add(link2);
            //Act
            Fixture.Diagram.SelectModel(link1, unselectAll);
            Fixture.Diagram.SelectModel(link2, unselectAll);
            //Assert
            if (unselectAll)
            {
                Assert.Single(Fixture.Diagram.GetSelectedModels());
                Assert.IsType<LinkModel>(Fixture.Diagram.GetSelectedModels().First());
            }
            else
            {
                Assert.Equal(2, Fixture.Diagram.GetSelectedModels().Count());
            }
           
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void SelectGroup(bool unselectAll)
        {
            //Arrange
            NodeModel node1 = new NodeModel();
            node1.AddPort();
            NodeModel node2 = new NodeModel();
            node2.AddPort();
            Fixture.Diagram.Nodes.Add(node1,node2);
            var group=Fixture.Diagram.Group(node1, node2);
            //Act
            Fixture.Diagram.SelectModel(group, unselectAll);
            //Assert

            Assert.Single(Fixture.Diagram.GetSelectedModels());
            Assert.IsType<GroupModel>(Fixture.Diagram.GetSelectedModels().First());
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void SelectSeveralGroups(bool unselectAll)
        {
            //Arrange
            NodeModel node1 = new NodeModel();
            node1.AddPort();
            NodeModel node2 = new NodeModel();
            node2.AddPort();
            NodeModel node3 = new NodeModel();
            node3.AddPort();
            NodeModel node4 = new NodeModel();
            node3.AddPort();
            Fixture.Diagram.Nodes.Add(node1,node2,node3,node4);

            var group1 = Fixture.Diagram.Group(node1, node2);
            var group2 = Fixture.Diagram.Group(node3, node4);
            //Act
            Fixture.Diagram.SelectModel(group1, unselectAll);
            Fixture.Diagram.SelectModel(group2, unselectAll);
            //Assert
            if (unselectAll)
            {
                Assert.Single(Fixture.Diagram.GetSelectedModels());
                Assert.IsType<GroupModel>(Fixture.Diagram.GetSelectedModels().First());
            }
            else
            {
                Assert.Equal(2, Fixture.Diagram.GetSelectedModels().Count());
            }

        }



        [Fact]
        public void UnselectAll()
        {
            //Arrange
            NodeModel node1 = new NodeModel();
            NodeModel node2 = new NodeModel();
            NodeModel node3 = new NodeModel(); 
            Fixture.Diagram.Nodes.Add(node1);
            Fixture.Diagram.Nodes.Add(node2);
            Fixture.Diagram.Nodes.Add(node3);
            Fixture.Diagram.SelectModel(node1, false);
            Fixture.Diagram.SelectModel(node2, false);
            //Act
            Fixture.Diagram.UnselectAll();
            //Assert
            Assert.Empty(Fixture.Diagram.GetSelectedModels());
            Assert.False(node1.Selected);
            Assert.False(node2.Selected);
            Assert.False(node3.Selected);

        }
        [Fact]
        public void NodeAddedEventTriggered()
        {
            //arrange
            AddNode();
            //Assert
            Assert.True(Fixture.NodeAddedEventTriggered);
        }
        [Fact]
        public void NodeRemovedEventTriggered()
        {
            //arrange
            RemoveNodes();
            //Assert
            Assert.True(Fixture.NodeRemovedEventTriggered);
        }
        [Fact]
        public void LinkAddedEventTriggered()
        {
            //arrange
            //act
            AddLink();
            //Assert
            Assert.True(Fixture.LinkAddedEventTriggered);
        }
        [Fact]
        public void LinkRemovedEventTriggered()
        {
            //arrange
            //act
            RemoveLink();
            //Assert
            Assert.True(Fixture.LinkRemovedEventTriggered);
        }
        [Fact]
        public void GroupAddedEventTriggered()
        {
            //arrange
            //act
            AddGroup();
            //Assert
            Assert.True(Fixture.GroupAddedEventTriggered);
        }
        [Fact]
        public void GroupRemovedEventTriggered()
        {
            //arrange
            //act
            RemoveGroup();
            //Assert
            Assert.True(Fixture.GroupRemovedEventTriggered);
        }
    }
}
