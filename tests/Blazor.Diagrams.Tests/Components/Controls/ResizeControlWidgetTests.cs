using Blazor.Diagrams.Components.Controls;
using Blazor.Diagrams.Core.Controls.Default;
using Blazor.Diagrams.Core.Positions.Resizing;
using Bunit;
using Moq;
using Xunit;

namespace Blazor.Diagrams.Tests.Components.Controls;

	public class ResizeControlWidgetTests
{
    [Fact]
    public void ShouldRenderDiv()
    {
        using var ctx = new TestContext();
        var providerMock = Mock.Of<IResizerProvider>();

        var cut = ctx.RenderComponent<ResizeControlWidget>(parameters =>
            parameters.Add(w => w.Control, new ResizeControl(providerMock))
        );

        cut.MarkupMatches("<div class=\"default-node-resizer\" />");
    }
}
