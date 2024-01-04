namespace Blazor.Diagrams.Core.Positions.Resizing
{
    public class BottomRightResizerProvider : ResizerProvider
    {
        override public string? Class => "bottomright";
        override public double HeightOffset => 5;
        override public double WidthOffset => 5;
        override public bool ShouldUseWidth => true;
        override public bool ShouldUseHeight => true;
        override public bool ShouldChangeXPositionOnResize => false;
        override public bool ShouldChangeYPositionOnResize => false;
        override public bool ShouldAddTotalMovedX => true;
        override public bool ShouldAddTotalMovedY => true;

    }
}
