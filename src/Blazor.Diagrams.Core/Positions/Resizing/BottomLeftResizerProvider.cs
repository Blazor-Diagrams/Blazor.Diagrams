namespace Blazor.Diagrams.Core.Positions.Resizing
{
    public class BottomLeftResizerProvider : ResizerProvider
    {
        override public string? Class => "bottomleft";

        override public double HeightOffset => 5;
        override public double WidthOffset => -5;
        override public bool ShouldUseWidth => false;
        override public bool ShouldUseHeight => true;
        override public bool ShouldChangeXPositionOnResize => true;
        override public bool ShouldChangeYPositionOnResize => false;
        override public bool ShouldAddTotalMovedX => false;
        override public bool ShouldAddTotalMovedY => true;

    }
}
