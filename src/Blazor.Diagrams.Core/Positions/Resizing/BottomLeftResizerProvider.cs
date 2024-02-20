using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;

namespace Blazor.Diagrams.Core.Positions.Resizing
{
    public class BottomLeftResizerProvider : ResizerProvider
    {
        override public string? Class => "bottomleft";
        override public bool ShouldChangeXPositionOnResize => true;
        override public bool ShouldChangeYPositionOnResize => false;
        override public bool ShouldAddTotalMovedX => false;
        override public bool ShouldAddTotalMovedY => true;

        public override Point? GetPosition(Model model)
        {
            if (model is NodeModel nodeModel && nodeModel.Size is not null)
            {
                return new Point(nodeModel.Position.X - 5, nodeModel.Position.Y + nodeModel.Size.Height + 5);
            }
            return null;
        }

    }
}
