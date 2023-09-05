using System;

namespace Blazor.Diagrams.Core;

public abstract class Behavior : IDisposable
{
    public Behavior(Diagram diagram)
    {
        Diagram = diagram;
    }

    protected Diagram Diagram { get; }

    public abstract void Dispose();
}
