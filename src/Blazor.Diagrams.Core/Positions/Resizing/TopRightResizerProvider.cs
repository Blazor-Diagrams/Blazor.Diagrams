namespace Blazor.Diagrams.Core.Positions.Resizing
{
    public class TopRightResizerProvider : ResizerProvider
    {
        override public string? Class => "topright";
        override public double HeightOffset => -5;
        override public double WidthOffset => 5;
        override public bool ShouldUseWidth => true;
        override public bool ShouldUseHeight => false;
        override public bool ShouldChangeXPositionOnResize => false;
        override public bool ShouldChangeYPositionOnResize => true;
        override public bool ShouldAddTotalMovedX => true;
        override public bool ShouldAddTotalMovedY => false;

    }
}
