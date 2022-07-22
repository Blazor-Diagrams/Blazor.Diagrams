using System;

namespace Blazor.Diagrams.Core
{
    public abstract class Behavior : IDisposable
    {
        public Behavior(DiagramBase diagram)
        {
            Diagram = diagram;
        }

        protected DiagramBase Diagram { get; }

        public abstract void Dispose();
    }
}
