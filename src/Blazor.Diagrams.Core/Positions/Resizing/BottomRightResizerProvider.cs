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
        private double? _lastClientX;
        private double? _lastClientY;
        private NodeModel _nodeModel = null!;
        private double _totalMovedX = 0;
        private double _totalMovedY = 0;

        public Point? GetPosition(Model model)
        {
            if (model is NodeModel nodeModel && nodeModel.Size is not null)
            {
                return new Point(nodeModel.Position.X + nodeModel.Size.Width + 5, nodeModel.Position.Y + nodeModel.Size.Height + 5);
            }
            return null;
        }

        public void OnResizeStart(Diagram diagram, Model model, PointerEventArgs e)
        {
            if (model is NodeModel nodeModel)
            {
                _lastClientX = e.ClientX;
                _lastClientY = e.ClientY;
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
            var width = _originalSize.Width + _totalMovedX;

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
            _nodeModel = null!;
            _totalMovedX = 0;
            _totalMovedY = 0;
            _lastClientX = null;
            _lastClientY = null;
        }

    }
}
