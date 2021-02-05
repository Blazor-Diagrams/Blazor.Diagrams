using System;

namespace Blazor.Diagrams.Core
{
    public abstract class Behavior : IDisposable
    {
        public Behavior(DiagramManager diagramManager)
        {
            DiagramManager = diagramManager;
        }

        protected DiagramManager DiagramManager { get; }

        public abstract void Dispose();
    }
}
