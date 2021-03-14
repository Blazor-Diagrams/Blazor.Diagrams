using System;

namespace Blazor.Diagrams.Core.Geometry
{
    public class Point
    {
        public static Point Zero { get; } = new Point(0, 0);

        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }

        public double X { get; }
        public double Y { get; }

        public double Dot(Point other) => X * other.X + Y * other.Y;
        public Point Lerp(Point other, double t)
            => new Point(X * (1.0 - t) + other.X * t, Y * (1.0 - t) + other.Y * t);

        // Maybe just make Points mutable?
        public Point Add(double value) => new Point(X + value, Y + value);
        public Point Add(double x, double y) => new Point(X + x, Y + y);

        public Point Substract(double value) => new Point(X - value, Y - value);
        public Point Substract(double x, double y) => new Point(X - x, Y - y);

        public double DistanceTo(Point other)
            => Math.Sqrt(Math.Pow(X - other.X, 2) + Math.Pow(Y - other.Y, 2));

        public override string ToString() => $"Point(x={X}, y={Y})";

        public static Point operator -(Point a, Point b) => new Point(a.X - b.X, a.Y - b.Y);
        public static Point operator +(Point a, Point b) => new Point(a.X + b.X, a.Y + b.Y);

        public void Deconstruct(out double x, out double y)
        {
            x = X;
            y = Y;
        }
    }
}
