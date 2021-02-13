using Blazor.Diagrams.Core.Models.Base;
using Blazor.Diagrams.Core.Models.Core;
using System;
using System.Text;

namespace Blazor.Diagrams.Core
{
    public static partial class PathGenerators
    {
        public static PathGeneratorResult Straight(DiagramManager _, BaseLinkModel link, Point[] route)
        {
            route = (Point[])route.Clone();
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

            var sb = new StringBuilder(FormattableString.Invariant($"M {route[0].X} {route[0].Y}"));

            for (var i = 1; i < route.Length; i++)
            {
                sb.Append(FormattableString.Invariant($" L {route[i].X} {route[i].Y}"));
            }

            return new PathGeneratorResult(sb.ToString(), sourceAngle, route[0], targetAngle, route[^1]);
        }
    }
}
