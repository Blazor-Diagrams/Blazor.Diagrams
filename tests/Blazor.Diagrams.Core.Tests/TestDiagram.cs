using Blazor.Diagrams.Core.Options;

namespace Blazor.Diagrams.Core.Tests;

public class TestDiagram : Diagram
{
    public TestDiagram(DiagramOptions? options = null)
    {
        Options = options ?? new DiagramOptions();
    }

    public override DiagramOptions Options { get; }
}