using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;

namespace Blazor.Diagrams.Core.Positions.Resizing
{
    public class TopLeftResizerProvider : ResizerProvider
    {
        override public string? Class => "topleft";
        override public bool ShouldChangeXPositionOnResize => true;
        override public bool ShouldChangeYPositionOnResize => true;
        override public bool ShouldAddTotalMovedX => false;
        override public bool ShouldAddTotalMovedY => false;

        public override Point? GetPosition(Model model)
        {
            if (model is NodeModel nodeModel && nodeModel.Size is not null)
            {
                return new Point(nodeModel.Position.X - 5, nodeModel.Position.Y - 5);
            }
            return null;
        }

    }
}
