using Blazor.Diagrams.Core.Extensions;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Xunit;

namespace Blazor.Diagrams.Core.Tests.Extensions;

public class DiagramExtensionsTests
{
    [Fact]
    public void GetBounds_ShouldReturnZeroRectangle_WhenNodesAreEmpty()
    {
        // Arrange
        var nodes = new NodeModel[0];

        // Act
        var bounds = nodes.GetBounds();

        // Assert
        Assert.True(Rectangle.Zero.Equals(bounds));
    }

    [Fact]
    public void GetBounds_ShouldReturnCorrectBounds()
    {
        // Arrange
        var nodes = new NodeModel[]
        {
            new NodeModel
            {
                Position = new Point(10, 10),
                Size = new Size(100, 100)
            },
            new NodeModel
            {
                Position = new Point(200, 200),
                Size = new Size(100, 100)
            },
        };

        // Act
        var bounds = nodes.GetBounds();

        // Assert
        var expected = new Rectangle(10, 10, 300, 300);
        Assert.True(expected.Equals(bounds));
    }
}
