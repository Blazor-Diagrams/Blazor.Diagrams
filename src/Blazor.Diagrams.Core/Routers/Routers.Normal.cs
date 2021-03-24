using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models.Base;
using System.Linq;

namespace Blazor.Diagrams.Core
{
    public static partial class Routers
    {
        public static Point[] Normal(Diagram _, BaseLinkModel link)
        {
            return link.Vertices.Select(v => v.Position).ToArray();
        }
    }
}
