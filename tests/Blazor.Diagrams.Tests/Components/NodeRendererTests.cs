using Blazor.Diagrams.Components.Renderers;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Bunit;
using Xunit;

namespace Blazor.Diagrams.Tests.Components;

public class NodeRendererTests
{

    [Fact]
    public void NodeRenderer_WhenNodeVisibleIsFalseAndIsFirstRender_ShouldNotObserveResizes()
    {
        // Arrange
        JSRuntimeInvocationHandler call;
        using (var ctx = new TestContext())
        {
            ctx.JSInterop.Setup<Rectangle>("ZBlazorDiagrams.getBoundingClientRect", _ => true);
            call = ctx.JSInterop.SetupVoid("ZBlazorDiagrams.observe", _ => true).SetVoidResult();

            // Act
            var cut = ctx.RenderComponent<NodeRenderer>(p =>
            {
                p.Add(n => n.BlazorDiagram, new BlazorDiagram());
                p.Add(n => n.Node, new NodeModel()
                {
                    Visible = false
                });
            });
        }

        // Assert
        call.VerifyNotInvoke("ZBlazorDiagrams.observe");
    }
}
