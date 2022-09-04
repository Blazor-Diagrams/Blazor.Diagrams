using Blazor.Diagrams.Core.Geometry;
using SvgPathProperties;
using System;

namespace Blazor.Diagrams.Core
{
    public class PathGeneratorResult
    {
        public static PathGeneratorResult Empty { get; } = new(Array.Empty<SvgPath>());

        public PathGeneratorResult(SvgPath[] paths, double? sourceMarkerAngle = null, Point? sourceMarkerPosition = null,
            double? targetMarkerAngle = null, Point? targetMarkerPosition = null)
        {
            Paths = paths;
            SourceMarkerAngle = sourceMarkerAngle;
            SourceMarkerPosition = sourceMarkerPosition;
            TargetMarkerAngle = targetMarkerAngle;
            TargetMarkerPosition = targetMarkerPosition;
        }

        public SvgPath[] Paths { get; }
        public double? SourceMarkerAngle { get; }
        public Point? SourceMarkerPosition { get; }
        public double? TargetMarkerAngle { get; }
        public Point? TargetMarkerPosition { get; }
    }
}
