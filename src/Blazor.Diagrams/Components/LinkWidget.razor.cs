using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Blazor.Diagrams.Core.Geometry;
using System.Collections.Generic;
using System;

namespace Blazor.Diagrams.Components
{
    public partial class LinkWidget
    {
        [CascadingParameter]
        public Diagram Diagram { get; set; }

        [Parameter]
        public LinkModel Link { get; set; }

        private void OnMouseDown(MouseEventArgs e, int index)
        {
            if (!Link.Segmentable)
                return;

            var rPt = Diagram.GetRelativeMousePoint(e.ClientX, e.ClientY);
            var vertex = new LinkVertexModel(Link, rPt);
            Link.Vertices.Insert(index, vertex);
            Diagram.OnMouseDown(vertex, e);
        }

        private (Point source, Point target) FindConnectionPoints(Point[] route)
        {
            if (Link.SourcePort == null) // Portless
            {
                if (Link.SourceNode.Size == null || Link.TargetNode?.Size == null)
                    return (null, null);

                var sourceCenter = Link.SourceNode.GetBounds().Center;
                var targetCenter = Link.TargetNode?.GetBounds().Center ?? Link.OnGoingPosition;
                var firstPt = route.Length > 0 ? route[0] : targetCenter;
                var secondPt = route.Length > 0 ? route[0] : sourceCenter;
                var sourceLine = new Line(firstPt, sourceCenter);
                var targetLine = new Line(secondPt, targetCenter);
                var sourceIntersections = Link.SourceNode.GetShape().GetIntersectionsWithLine(sourceLine);
                var targetIntersections = Link.TargetNode.GetShape().GetIntersectionsWithLine(targetLine);
                var sourceIntersection = GetClosestPointTo(sourceIntersections, firstPt);
                var targetIntersection = GetClosestPointTo(targetIntersections, secondPt);
                return (sourceIntersection ?? sourceCenter, targetIntersection ?? targetCenter);
            }
            else
            {
                if (!Link.SourcePort.Initialized || Link.TargetPort?.Initialized == false)
                    return (null, null);

                return (Link.SourcePort.MiddlePosition, Link.TargetPort?.MiddlePosition ?? Link.OnGoingPosition);
            }
        }

        private Point GetClosestPointTo(IEnumerable<Point> points, Point point)
        {
            var minDist = double.MaxValue;
            Point minPoint = null;

            foreach (var pt in points)
            {
                var dist = pt.DistanceTo(point);
                if (dist < minDist)
                {
                    minDist = dist;
                    minPoint = pt;
                }
            }

            return minPoint;
        }
    }
}
