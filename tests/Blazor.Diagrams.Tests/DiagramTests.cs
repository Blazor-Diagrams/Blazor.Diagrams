using Blazor.Diagrams.Components;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;
using FluentAssertions;
using Microsoft.AspNetCore.Components;
using Xunit;

namespace Blazor.Diagrams.Tests
{
    public class DiagramTests
    {
        [Fact]
        public void GetComponentForModel_ShouldReturnComponentType_WhenModelTypeWasRegistered()
        {
            // Arrange
            var diagram = new Diagram();
            diagram.RegisterModelComponent<NodeModel, NodeWidget>();

            // Act
            var componentType = diagram.GetComponentForModel<NodeModel>();

            // Assert
            componentType.Should().Be(typeof(NodeWidget));
        }

        [Fact]
        public void GetComponentForModel_ShouldReturnNull_WhenModelTypeWasNotRegistered()
        {
            // Arrange
            var diagram = new Diagram();

            // Act
            var componentType = diagram.GetComponentForModel<NodeModel>();

            // Assert
            componentType.Should().BeNull();
        }

        [Fact]
        public void GetComponentForModel_ShouldReturnComponentType_WhenInheritedModelTypeWasRegistered()
        {
            // Arrange
            var diagram = new Diagram();
            diagram.RegisterModelComponent<Model, NodeWidget>();

            // Act
            var componentType = diagram.GetComponentForModel<CustomModel>();

            // Assert
            componentType.Should().Be(typeof(NodeWidget));
        }

        [Fact]
        public void GetComponentForModel_ShouldReturnSpecificComponentType_WhenInheritedAndSpecificModelTypeWasRegistered()
        {
            // Arrange
            var diagram = new Diagram();
            diagram.RegisterModelComponent<CustomModel, CustomWidget>();
            diagram.RegisterModelComponent<Model, NodeWidget>();

            // Act
            var componentType = diagram.GetComponentForModel<CustomModel>();

            // Assert
            componentType.Should().Be(typeof(CustomWidget));
        }

        [Fact]
        public void GetComponentForModel_ShouldReturnNull_WhenCheckSubclassesIsFalse()
        {
            // Arrange
            var diagram = new Diagram();
            diagram.RegisterModelComponent<Model, NodeWidget>();

            // Act
            var componentType = diagram.GetComponentForModel<CustomModel>(false);

            // Assert
            componentType.Should().BeNull();
        }

        private class CustomModel : Model { }
        private class CustomWidget : ComponentBase { }
    }
}
