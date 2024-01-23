using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;

namespace Blazor.Diagrams.Core.Positions.Resizing
{
    public class BottomRightResizerProvider : ResizerProvider
    {
        override public string? Class => "bottomright";
        override public bool ShouldChangeXPositionOnResize => false;
        override public bool ShouldChangeYPositionOnResize => false;
        override public bool ShouldAddTotalMovedX => true;
        override public bool ShouldAddTotalMovedY => true;

        public override Point? GetPosition(Model model)
        {
            if (model is NodeModel nodeModel && nodeModel.Size is not null)
            {
                return new Point(nodeModel.Position.X + nodeModel.Size.Width + 5, nodeModel.Position.Y + nodeModel.Size.Height + 5);
            }
            return null;
        }

    }
}
