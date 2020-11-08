using Blazor.Diagrams.Core.Models.Core;
using System;

namespace Blazor.Diagrams.Algorithms.Extensions
{
    public static class PointExtensions
    {
        public static double DistanceTo(this Point firstPoint, Point secondPoint)
        {
            var x = Math.Abs(firstPoint.X - secondPoint.X);
            var y = Math.Abs(firstPoint.Y - secondPoint.Y);
            return Math.Sqrt(x * x + y * y);
        }
    }
}
