using Blazor.Diagrams.Core.Extensions;

namespace Blazor.Diagrams.Core.Models.Core
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

        public Size Add(double value) => new Size(Width + value, Height + value);

        public bool Equals(Size? size) => size != null && Width.AlmostEqualTo(size.Width) && Height.AlmostEqualTo(size.Height);

        public override string ToString() => $"Size(width={Width}, height={Height})";
    }
}
