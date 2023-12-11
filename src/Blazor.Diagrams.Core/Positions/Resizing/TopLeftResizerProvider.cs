using Blazor.Diagrams.Core.Events;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;

namespace Blazor.Diagrams.Core.Positions.Resizing
{
    public class TopLeftResizerProvider : IResizerProvider
    {
		public string? Class => "topleft";

		private Size _originalSize = null!;
        private Point _originalPosition = null!;
        private Point _originalMousePosition = null!;
        private NodeModel _nodeModel = null!;

        public Point? GetPosition(Model model)
        {
            if (model is NodeModel nodeModel && nodeModel.Size is not null)
            {
                return new Point(nodeModel.Position.X - 5, nodeModel.Position.Y - 5);
            }
            return null;
        }

        public void OnResizeStart(Diagram diagram, Model model, PointerEventArgs eventArgs)
        {
            if (model is NodeModel nodeModel)
            {
                _originalPosition = new Point(nodeModel.Position.X, nodeModel.Position.Y);
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

            var height = _originalSize.Height - (args.ClientY - _originalMousePosition.Y);
            var width = _originalSize.Width - (args.ClientX - _originalMousePosition.X);

            var positionX = _originalPosition.X + (args.ClientX - _originalMousePosition.X);
            var positionY = _originalPosition.Y + (args.ClientY - _originalMousePosition.Y);

            if (width < _nodeModel.MinimumDimensions.Width)
            {
                width = _nodeModel.MinimumDimensions.Width;
                positionX = _nodeModel.Position.X;
            }
            if (height < _nodeModel.MinimumDimensions.Height)
            {
                height = _nodeModel.MinimumDimensions.Height;
                positionY = _nodeModel.Position.Y;
            }

            _nodeModel.SetPosition(positionX, positionY);
            _nodeModel.SetSize(width, height);
        }

        public void OnResizeEnd(Model? model, PointerEventArgs args)
        {
            _nodeModel?.TriggerSizeChanged();
            _originalSize = null!;
            _originalPosition = null!;
            _originalMousePosition = null!;
            _nodeModel = null!;
        }

    }
}
