namespace Blazor.Diagrams.Core.Models
{
    public class Size
    {
        public static Size Zero { get; } = new Size(0, 0);

        public Size(double width, double height)
        {
            Width = width;
            Height = height;
        }

        public double Width { get; }
        public double Height { get; }

        public override string ToString() => $"Size(width={Width}, height={Height})";
    }
}
