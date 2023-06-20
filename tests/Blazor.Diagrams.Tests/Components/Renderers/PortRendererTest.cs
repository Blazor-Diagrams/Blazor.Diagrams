using Blazor.Diagrams.Components.Renderers;
using Blazor.Diagrams.Core.Models;
using Bunit;
using Xunit;

namespace Blazor.Diagrams.Tests.Components.Renderers;

public class PortRendererTest
{
    [Fact]
    void UpdateDimensionsDoesNotCrashWithNullContainer()
    {
        using var ctx = new TestContext();
        ctx.JSInterop.Mode = JSRuntimeMode.Loose;
        var node = new NodeModel();
        var portModel = new PortModel(node, PortAlignment.Bottom);

        var component = ctx.RenderComponent<PortRenderer>(parameters => parameters
            .Add(n => n.Port, portModel)
            .Add(n => n.BlazorDiagram, new BlazorDiagram()));
    }
}