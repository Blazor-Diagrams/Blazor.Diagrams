using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models.Base;
using SvgPathProperties;

namespace Blazor.Diagrams.Core
{
    public static partial class PathGenerators
    {
        public static PathGeneratorResult Straight(Diagram _, BaseLinkModel link, Point[] route, Point source, Point target)
        {
            route = ConcatRouteAndSourceAndTarget(route, source, target);
            double? sourceAngle = null;
            double? targetAngle = null;

            if (link.SourceMarker != null)
            {
                sourceAngle = SourceMarkerAdjustement(route, link.SourceMarker.Width);
            }

            if (link.TargetMarker != null)
            {
                targetAngle = TargetMarkerAdjustement(route, link.TargetMarker.Width);
            }

            var paths = new SvgPath[route.Length - 1];
            for (var i = 0; i < route.Length - 1; i++)
            {
                paths[i] = new SvgPath().AddMoveTo(route[i].X, route[i].Y).AddLineTo(route[i + 1].X, route[i + 1].Y);
            }

            return new PathGeneratorResult(paths, sourceAngle, route[0], targetAngle, route[^1]);
        }
    }
}
