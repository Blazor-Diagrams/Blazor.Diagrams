using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Core;
using System;
using System.Text;

namespace Blazor.Diagrams.Core
{
    public static partial class PathGenerators
    {
        private const double _margin = 125;

        public static string Smooth(DiagramManager _, LinkModel link, Point[] route)
        {
            var sb = new StringBuilder(FormattableString.Invariant($"M {route[0].X} {route[0].Y}"));

            for (var i = 1; i < route.Length; i++)
            {
                // Todo: alignments should be null for middle segments
                sb.Append(GenerateCurvedPath(route[i - 1].X, route[i - 1].Y, route[i].X, route[i].Y,
                    link.SourcePort.Alignment, link.TargetPort?.Alignment));
            }

            return sb.ToString();
        }

        private static string GenerateCurvedPath(double sX, double sY, double tX, double tY,
            PortAlignment sourcePortAlignment, PortAlignment? targetPortAlignment)
        {
            var cX = (sX + tX) / 2;
            var cY = (sY + tY) / 2;
            var curvePointA = GetCurvePoint(sX, sY, cX, cY, sourcePortAlignment);
            var curvePointB = GetCurvePoint(tX, tY, cX, cY, targetPortAlignment);
            return FormattableString.Invariant($" C {curvePointA}, {curvePointB}, {tX} {tY}");
        }

        private static string GetCurvePoint(double pX, double pY, double cX, double cY, PortAlignment? alignment)
        {
            var margin = Math.Min(_margin, Math.Pow(Math.Pow(pX - cX, 2) + Math.Pow(pY - cY, 2), .5));
            return alignment switch
            {
                PortAlignment.Top => FormattableString.Invariant($"{pX} {Math.Min(pY - margin, cY)}"),
                PortAlignment.Bottom => FormattableString.Invariant($"{pX} {Math.Max(pY + margin, cY)}"),
                PortAlignment.TopRight => FormattableString.Invariant($"{Math.Max(pX + margin, cX)} {Math.Min(pY - margin, cY)}"),
                PortAlignment.BottomRight => FormattableString.Invariant($"{Math.Max(pX + margin, cX)} {Math.Max(pY + margin, cY)}"),
                PortAlignment.Right => FormattableString.Invariant($"{Math.Max(pX + margin, cX)} {pY}"),
                PortAlignment.Left => FormattableString.Invariant($"{Math.Min(pX - margin, cX)} {pY}"),
                PortAlignment.BottomLeft => FormattableString.Invariant($"{Math.Min(pX - margin, cX)} {Math.Max(pY + margin, cY)}"),
                PortAlignment.TopLeft => FormattableString.Invariant($"{Math.Min(pX - margin, cX)} {Math.Min(pY - margin, cY)}"),
                _ => FormattableString.Invariant($"{cX} {cY}"),
            };
        }
    }
}
