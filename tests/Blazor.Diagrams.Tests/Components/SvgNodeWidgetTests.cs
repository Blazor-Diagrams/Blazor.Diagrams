using Blazor.Diagrams.Components;

using Bunit;
using Xunit;

namespace Blazor.Diagrams.Tests.Components;

public class SvgNodeWidgetTests
{
    [Fact]
    public void ShouldRenderSimpleRect()
    {
        // Arrange
        using var ctx = new TestContext();

        // Act
        var cut = ctx.RenderComponent<SvgNodeWidget>();

        // Assert
        cut.MarkupMatches("<rect width=\"50\" height=\"50\" style=\"fill:rgb(0, 0, 255); stroke-width:3; stroke:rgb(0, 0, 0)\" />");
    }
}
