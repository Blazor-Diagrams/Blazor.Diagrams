namespace Blazor.Diagrams.Core.Positions.Resizing
{
    public class TopLeftResizerProvider : ResizerProvider
    {
        override public string? Class => "topleft";

        override public double HeightOffset => -5;
        override public double WidthOffset => -5;
        override public bool ShouldUseWidth => false;
        override public bool ShouldUseHeight => false;
        override public bool ShouldChangeXPositionOnResize => true;
        override public bool ShouldChangeYPositionOnResize => true;
        override public bool ShouldAddTotalMovedX => false;
        override public bool ShouldAddTotalMovedY => false;

    }
}
