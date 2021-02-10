using Blazor.Diagrams.Core.Models.Core;

namespace Blazor.Diagrams.Core
{
    public class PathGeneratorResult
    {
        public PathGeneratorResult(string path, double? sourceMarkerAngle = null, Point? sourceMarkerPosition = null,
            double? targetMarkerAngle = null, Point? targetMarkerPosition = null)
        {
            Path = path;
            SourceMarkerAngle = sourceMarkerAngle;
            SourceMarkerPosition = sourceMarkerPosition;
            TargetMarkerAngle = targetMarkerAngle;
            TargetMarkerPosition = targetMarkerPosition;
        }

        public string Path { get; }
        public double? SourceMarkerAngle { get; }
        public Point? SourceMarkerPosition { get; }
        public double? TargetMarkerAngle { get; }
        public Point? TargetMarkerPosition { get; }
    }
}
