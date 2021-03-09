using Blazor.Diagrams.Core.Models.Base;
using Blazor.Diagrams.Core.Models.Core;
using System;
using System.Linq;

namespace Blazor.Diagrams.Core
{
    public static partial class Routers
    {
        public static Point[] Normal(Diagram _, BaseLinkModel link)
        {
            var route = new Point[link.Vertices.Count + 2];
            route[0] = link.SourceMarker == null ? link.SourcePort.MiddlePosition : GetPortPositionBasedOnAlignment(link.SourcePort);

            if (link.Vertices.Count > 0)
            {
                Array.Copy(link.Vertices.Select(v => v.Position).ToArray(), 0, route, 1, link.Vertices.Count);
            }

            route[^1] = link.TargetPort == null
                ? link.OnGoingPosition!
                : link.TargetMarker == null ? link.TargetPort.MiddlePosition : GetPortPositionBasedOnAlignment(link.TargetPort);
            return route;
        }
    }
}
