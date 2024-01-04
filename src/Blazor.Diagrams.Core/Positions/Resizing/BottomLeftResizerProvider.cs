using Blazor.Diagrams.Core.Events;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;

namespace Blazor.Diagrams.Core.Positions.Resizing
{
    public class BottomLeftResizerProvider : IResizerProvider
    {
        public string? Class => "bottomleft";

        private Size _originalSize = null!;
        private Point _originalPosition = null!;
        private NodeModel _nodeModel = null!;
        private double? _lastClientX;
        private double? _lastClientY;
        private double _totalMovedX = 0;
        private double _totalMovedY = 0;

        public Point? GetPosition(Model model)
        {
            if (model is NodeModel nodeModel && nodeModel.Size is not null)
            {
                return new Point(nodeModel.Position.X - 5, nodeModel.Position.Y + nodeModel.Size.Height + 5);
            }
            return null;
        }

        public void OnResizeStart(Diagram diagram, Model model, PointerEventArgs e)
        {
            if (model is NodeModel nodeModel)
            {
                _lastClientX = e.ClientX;
                _lastClientY = e.ClientY;
                _originalPosition = new Point(nodeModel.Position.X, nodeModel.Position.Y);
                _originalSize = nodeModel.Size!;
                _nodeModel = nodeModel;
            }
        }

        public void OnPointerMove(Model? model, PointerEventArgs e)
        {
            if (_nodeModel is null) return;

            var deltaX = (e.ClientX - _lastClientX!.Value);
            var deltaY = (e.ClientY - _lastClientY!.Value);

            _lastClientX = e.ClientX;
            _lastClientY = e.ClientY;

            ResizeNode(deltaX, deltaY);
        }

        public void OnPanChanged(double deltaX, double deltaY, double clientX, double clientY)
        {
            if (_nodeModel is null) return;

            ResizeNode(deltaX, deltaY);
        }

        public void ResizeNode(double deltaX, double deltaY)
        {
            _totalMovedX += deltaX;
            _totalMovedY += deltaY;

            var height = _originalSize.Height + _totalMovedY;
            var width = _originalSize.Width - _totalMovedX;

            var positionX = _originalPosition.X + _totalMovedX;
            var positionY = _originalPosition.Y;

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
            _totalMovedY = 0;
            _lastClientX = null;
            _lastClientY = null;
            _nodeModel = null!;
        }

    }
}
