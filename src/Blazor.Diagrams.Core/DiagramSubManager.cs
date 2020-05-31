using System;

namespace Blazor.Diagrams.Core
{
    public abstract class DiagramSubManager : IDisposable
    {
        public DiagramSubManager(DiagramManager diagramManager)
        {
            DiagramManager = diagramManager;
        }

        protected DiagramManager DiagramManager { get; }

        public abstract void Dispose();
    }
}
