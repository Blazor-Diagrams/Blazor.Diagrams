using System;

namespace Blazor.Diagrams.Core.Models.Core
{
    public class Rectangle
    {
        public static Rectangle Zero { get; } = new Rectangle(0, 0, 0, 0);

        public double Width { get; set; }
        public double Height { get; set; }
        public double Top { get; set; }
        public double Right { get; set; }
        public double Bottom { get; set; }
        public double Left { get; set; }

        public Rectangle()
        {

        }

        public Rectangle(double left, double top, double right, double bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
            Width = Math.Abs(Left - Right);
            Height = Math.Abs(Top - Bottom);
        }

        public Rectangle(Point position, Size size)
        {
            Left = position.X;
            Top = position.Y;
            Right = Left + size.Width;
            Bottom = Top + size.Height;
            Width = size.Width;
            Height = size.Height;
        }

        public bool Overlap(Rectangle r)
            => Left < r.Right && Right > r.Left && Top < r.Bottom && Bottom > r.Top;

        public bool Intersects(Rectangle r)
        {
            var thisX = Left;
            var thisY = Top;
            var thisW = Width;
            var thisH = Height;
            var rectX = r.Left;
            var rectY = r.Top;
            var rectW = r.Width;
            var rectH = r.Height;
            return (rectX < thisX + thisW) && (thisX < (rectX + rectW)) && (rectY < thisY + thisH) && (thisY < rectY + rectH);
        }

        public Rectangle Inflate(double horizontal, double vertical)
            => new Rectangle(Left - horizontal, Top - vertical, Right + horizontal, Bottom + vertical);

        public Rectangle Union(Rectangle r)
        {
            var x1 = Math.Min(Left, r.Left);
            var x2 = Math.Max(Left + Width, r.Left + r.Width);
            var y1 = Math.Min(Top, r.Top);
            var y2 = Math.Max(Top + Height, r.Top + r.Height);
            return new Rectangle(x1, y1, x2, y2);
        }

        public bool ContainsPoint(Point point) => ContainsPoint(point.X, point.Y);

        public bool ContainsPoint(double x, double y)
            => x >= Left && x <= Right && y >= Top && y <= Bottom;

        public Point Center => new Point(Left + Width / 2, Top + Height / 2);
        public Point NorthEast => new Point(Right, Top);
        public Point SouthEast => new Point(Right, Bottom);
        public Point SouthWest => new Point(Left, Bottom);
        public Point NorthWest => new Point(Left, Top);
        public Point East => new Point(Right, Top + Height / 2);
        public Point North => new Point(Left + Width / 2, Top);
        public Point South => new Point(Left + Width / 2, Bottom);
        public Point West => new Point(Left, Top + Height / 2);

        public override string ToString()
                    => $"Rectangle(width={Width}, height={Height}, top={Top}, right={Right}, bottom={Bottom}, left={Left})";
    }
}
