using Blazor.Diagrams.Core.Events;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;

namespace Blazor.Diagrams.Core.Positions.Resizing
{
    public class BottomRightResizerProvider : IResizerProvider
    {
        public string? Class => "bottomright";

        private Size _originalSize = null!;
        private Point _originalMousePosition = null!;
        private NodeModel _nodeModel = null!;

		public Point? GetPosition(Model model)
        {
            if (model is NodeModel nodeModel && nodeModel.Size is not null)
            {
                return new Point(nodeModel.Position.X + nodeModel.Size.Width + 5, nodeModel.Position.Y + nodeModel.Size.Height + 5);
            }
            return null;
        }

        public void OnResizeStart(Diagram diagram, Model model, PointerEventArgs eventArgs)
        {
            if (model is NodeModel nodeModel)
            {
                _originalMousePosition = new Point(eventArgs.ClientX, eventArgs.ClientY);
                _originalSize = nodeModel.Size!;
                _nodeModel = nodeModel;
            }
        }

        public void OnPointerMove(Model? model, PointerEventArgs args)
        {
            if (_nodeModel is null)
            {
                return;
            }

            var height = _originalSize.Height + (args.ClientY - _originalMousePosition.Y);
            var width = _originalSize.Width + (args.ClientX - _originalMousePosition.X);

            if (width < _nodeModel.MinimumDimensions.Width)
            {
                width = _nodeModel.MinimumDimensions.Width;
            }
            if (height < _nodeModel.MinimumDimensions.Height)
            {
                height = _nodeModel.MinimumDimensions.Height;
            }

            _nodeModel.SetSize(width, height);
        }

        public void OnResizeEnd(Model? model, PointerEventArgs args)
        {
            _nodeModel?.TriggerSizeChanged();
            _originalSize = null!;
            _originalMousePosition = null!;
            _nodeModel = null!;
        }

    }
}
