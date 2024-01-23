using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;

namespace Blazor.Diagrams.Core.Positions.Resizing
{
    public class TopRightResizerProvider : ResizerProvider
    {
        override public string? Class => "topright";
        override public bool ShouldChangeXPositionOnResize => false;
        override public bool ShouldChangeYPositionOnResize => true;
        override public bool ShouldAddTotalMovedX => true;
        override public bool ShouldAddTotalMovedY => false;

        public override Point? GetPosition(Model model)
        {
            if (model is NodeModel nodeModel && nodeModel.Size is not null)
            {
                return new Point(nodeModel.Position.X + nodeModel.Size.Width + 5, nodeModel.Position.Y - 5);
            }
            return null;
        }

    }
}