namespace Blazor.Diagrams.Core.Models
{
    public class Size
    {
        public static Size Zero { get; } = new Size(0, 0);

        public Size() { }

        public Size(double width, double height)
        {
            Width = width;
            Height = height;
        }

        public double Width { get; set; }
        public double Height { get; set; }

        public override string ToString() => $"Size(width={Width}, height={Height})";
    }
}
