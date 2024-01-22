namespace Blazor.Diagrams.Tests.Components.Controls;

public class ResizeControlWidgetTests
{
    [Fact]
    public void ShouldRenderDiv()
    {
        using var ctx = new TestContext();
        var providerMock = Mock.Of<ResizerProvider>();

        var cut = ctx.RenderComponent<ResizeControlWidget>(parameters =>
            parameters.Add(w => w.Control, new ResizeControl(providerMock))
        );

        cut.MarkupMatches("<div class=\"default-node-resizer\" />");
    }
}
