namespace Blazor.Diagrams.Core.Models
{
    public class Point
    {
        public static readonly Point Zero = new Point(0, 0);

        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }

        public double X { get; }
        public double Y { get; }
    }
}
