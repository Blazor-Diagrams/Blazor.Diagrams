using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models.Base;
using System.Linq;

namespace Blazor.Diagrams.Core.Routers;

public class NormalRouter : Router
{
    public override Point[] GetRoute(Diagram diagram, BaseLinkModel link)
    {
        return link.Vertices.Select(v => v.Position).ToArray();
    }
}
