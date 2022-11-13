using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models.Base;
using SvgPathProperties;
using System;

namespace Blazor.Diagrams.Core.PathGenerators
{
    public class StraightPathGenerator : PathGenerator
    {
        public override PathGeneratorResult GetResult(Diagram diagram, BaseLinkModel link, Point[] route, Point source, Point target)
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

            if (link.Vertices.Count == 0)
            {
                var fullPath = new SvgPath().AddMoveTo(route[0].X, route[0].Y);

                for (var i = 0; i < route.Length - 1; i++)
                {
                    fullPath.AddLineTo(route[i + 1].X, route[i + 1].Y);
                }

                return new PathGeneratorResult(fullPath, Array.Empty<SvgPath>(), sourceAngle, route[0], targetAngle, route[^1]);
            }
            else
            {
                var paths = new SvgPath[route.Length - 1];
                var fullPath = new SvgPath().AddMoveTo(route[0].X, route[0].Y);

                for (var i = 0; i < route.Length - 1; i++)
                {
                    fullPath.AddLineTo(route[i + 1].X, route[i + 1].Y);
                    paths[i] = new SvgPath().AddMoveTo(route[i].X, route[i].Y).AddLineTo(route[i + 1].X, route[i + 1].Y);
                }

                return new PathGeneratorResult(fullPath, paths, sourceAngle, route[0], targetAngle, route[^1]);
            }
        }
    }
}
