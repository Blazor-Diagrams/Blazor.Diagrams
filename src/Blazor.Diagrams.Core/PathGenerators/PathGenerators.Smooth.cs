using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Core;
using System;

namespace Blazor.Diagrams.Core
{
    public static partial class PathGenerators
    {
        private const double _margin = 125;

        public static PathGeneratorResult Smooth(DiagramManager _, LinkModel link, Point[] route)
        {
            if (route.Length > 2)
                throw new NotImplementedException(); // Curved path through points

            route = GetRouteWithCurvePoints(link, route);
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

            var path = FormattableString.Invariant($"M {route[0].X} {route[0].Y} C {route[1].X} {route[1].Y}, {route[2].X} {route[2].Y}, {route[3].X} {route[3].Y}");
            return new PathGeneratorResult(path, sourceAngle, route[0], targetAngle, route[^1]);
        }

        private static Point[] GetRouteWithCurvePoints(LinkModel link, Point[] route)
        {
            var cX = (route[0].X + route[1].X) / 2;
            var cY = (route[0].Y + route[1].Y) / 2;
            var curvePointA = GetCurvePoint(route[0].X, route[0].Y, cX, cY, link.SourcePort.Alignment);
            var curvePointB = GetCurvePoint(route[1].X, route[1].Y, cX, cY, link.TargetPort?.Alignment);
            return new[] { route[0], curvePointA, curvePointB, route[1] };
        }

        private static Point GetCurvePoint(double pX, double pY, double cX, double cY, PortAlignment? alignment)
        {
            var margin = Math.Min(_margin, Math.Pow(Math.Pow(pX - cX, 2) + Math.Pow(pY - cY, 2), .5));
            return alignment switch
            {
                PortAlignment.Top => new Point(pX, Math.Min(pY - margin, cY)),
                PortAlignment.Bottom => new Point(pX, Math.Max(pY + margin, cY)),
                PortAlignment.TopRight => new Point(Math.Max(pX + margin, cX), Math.Min(pY - margin, cY)),
                PortAlignment.BottomRight => new Point(Math.Max(pX + margin, cX), Math.Max(pY + margin, cY)),
                PortAlignment.Right => new Point(Math.Max(pX + margin, cX), pY),
                PortAlignment.Left => new Point(Math.Min(pX - margin, cX), pY),
                PortAlignment.BottomLeft => new Point(Math.Min(pX - margin, cX), Math.Max(pY + margin, cY)),
                PortAlignment.TopLeft => new Point(Math.Min(pX - margin, cX), Math.Min(pY - margin, cY)),
                _ => new Point(cX, cY),
            };
        }
    }
}
