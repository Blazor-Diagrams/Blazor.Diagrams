using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models.Base;
using SvgPathProperties;
using System;

namespace Blazor.Diagrams.Core.PathGenerators
{
    public class StraightPathGenerator : PathGenerator
    {
        private readonly double _radius;

        public StraightPathGenerator(double radius = 20)
        {
            _radius = radius;
        }

        public override PathGeneratorResult GetResult(Diagram diagram, BaseLinkModel link, Point[] route, Point source, Point target)
        {
            route = ConcatRouteAndSourceAndTarget(route, source, target);

            double? sourceAngle = null;
            double? targetAngle = null;

            if (link.SourceMarker != null)
            {
                sourceAngle = AdjustRouteForSourceMarker(route, link.SourceMarker.Width);
            }

            if (link.TargetMarker != null)
            {
                targetAngle = AdjustRouteForTargetMarker(route, link.TargetMarker.Width);
            }

            var paths = link.Vertices.Count > 0 ? new SvgPath[route.Length - 1] : null;
            var fullPath = new SvgPath().AddMoveTo(route[0].X, route[0].Y);
            double? secondDist = null;

            for (var i = 0; i < route.Length - 1; i++)
            {
                if (_radius > 0 && i > 0)
                {
                    var previous = route[i - 1];
                    var current = route[i];
                    var next = route[i + 1];

                    double? firstDist = secondDist ?? (current.DistanceTo(previous) / 2);
                    secondDist = current.DistanceTo(next) / 2;

                    var p1 = -Math.Min(_radius, firstDist.Value);
                    var p2 = -Math.Min(_radius, secondDist.Value);

                    var fp = current.MoveAlongLine(previous, p1);
                    var sp = current.MoveAlongLine(next, p2);

                    var sweepFlag = GetAngle(previous, current, next) == 270;
                    fullPath.AddLineTo(fp.X, fp.Y).AddArc(_radius, _radius, 0, false, sweepFlag, sp.X, sp.Y);

                    if (i == route.Length - 2)
                    {
                        fullPath.AddLineTo(route[^1].X, route[^1].Y);
                    }
                }
                else if (_radius == 0 || route.Length == 2)
                {
                    fullPath.AddLineTo(route[i + 1].X, route[i + 1].Y);

                    if (paths != null)
                    {
                        paths[i] = new SvgPath().AddMoveTo(route[i].X, route[i].Y).AddLineTo(route[i + 1].X, route[i + 1].Y);
                    }
                }
            }

            return new PathGeneratorResult(fullPath, paths ?? Array.Empty<SvgPath>(), sourceAngle, route[0], targetAngle, route[^1]);
        }

        private static double GetAngle(Point prev, Point curr, Point next)
        {
            var r = Math.Atan2(next.Y - curr.Y, next.X - curr.X) - Math.Atan2(prev.Y - curr.Y, prev.X - curr.X);
            var ang = r * (180 / Math.PI);
            return ang < 0 ? ang + 360 : ang;
        }
    }
}
