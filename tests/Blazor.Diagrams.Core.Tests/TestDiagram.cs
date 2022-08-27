using Blazor.Diagrams.Core.Options;

namespace Blazor.Diagrams.Core.Tests;

public class TestDiagram : Diagram
{
    public TestDiagram()
    {
        Options = new DiagramOptions();
    }

    public override DiagramOptions Options { get; }
}