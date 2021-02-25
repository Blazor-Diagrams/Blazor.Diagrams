using System;

namespace Blazor.Diagrams.Core.Models.Core
{
    public class Rectangle
    {
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

        public bool ContainsPoint(Point point) => ContainsPoint(point.X, point.Y);

        public bool ContainsPoint(double x, double y)
            => x >= Left && x <= Right && y >= Top && y <= Bottom;

        public override string ToString()
            => $"Rectangle(width={Width}, height={Height}, top={Top}, right={Right}, bottom={Bottom}, left={Left})";
    }
}
