namespace Blazor.Diagrams.Core.Models
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

        // Maybe just make Points mutable?
        public Point Add(double x, double y) => new Point(X + x, Y + y);

        public override string ToString() => $"Point(x={X}, y={Y})";
    }
}
