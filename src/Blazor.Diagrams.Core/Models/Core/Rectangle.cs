namespace Blazor.Diagrams.Core.Models.Core
{
    public class Rectangle
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public double Top { get; set; }
        public double Right { get; set; }
        public double Bottom { get; set; }
        public double Left { get; set; }

        public override string ToString()
            => $"Rectangle(x={X}, y={Y}, width={Width}, height={Height}, top={Top}, right={Right}, bottom={Bottom}, left={Left})";
    }
}
