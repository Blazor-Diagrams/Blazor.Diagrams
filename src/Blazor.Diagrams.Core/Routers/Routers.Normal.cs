using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Core;

namespace Blazor.Diagrams.Core
{
    public static partial class Routers
    {
        public static Point[] Normal(DiagramManager _, LinkModel link, Point from, Point to)
            => new[] { from, to };
    }
}
