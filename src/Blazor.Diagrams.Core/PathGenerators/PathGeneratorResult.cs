using Blazor.Diagrams.Core.Geometry;

namespace Blazor.Diagrams.Core
{
    public class PathGeneratorResult
    {
        public PathGeneratorResult(string[] paths, double? sourceMarkerAngle = null, Point? sourceMarkerPosition = null,
            double? targetMarkerAngle = null, Point? targetMarkerPosition = null)
        {
            Paths = paths;
            SourceMarkerAngle = sourceMarkerAngle;
            SourceMarkerPosition = sourceMarkerPosition;
            TargetMarkerAngle = targetMarkerAngle;
            TargetMarkerPosition = targetMarkerPosition;
        }

        public string[] Paths { get; }
        public double? SourceMarkerAngle { get; }
        public Point? SourceMarkerPosition { get; }
        public double? TargetMarkerAngle { get; }
        public Point? TargetMarkerPosition { get; }
    }
}
