using Blazor.Diagrams.Core.Events;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;

namespace Blazor.Diagrams.Core.Positions.Resizing
{
    public abstract class ResizerProvider : IPositionProvider
    {
        abstract public string? Class { get; }

        protected Size? _originalSize { get; set; }
        protected Point? _originalPosition { get; set; }
        private double? _lastClientX;
        private double? _lastClientY;
        protected NodeModel? _nodeModel { get; set; }
        private double _totalMovedX = 0;
        private double _totalMovedY = 0;
        protected Diagram? _diagram { get; set; }

        abstract public bool ShouldChangeXPositionOnResize { get; }
        abstract public bool ShouldChangeYPositionOnResize { get; }
        ///<summary> Controls whether the totalMovedX should be added or subtracted </summary>
        abstract public bool ShouldAddTotalMovedX { get; }
        ///<summary> Controls whether the totalMovedY should be added or subtracted </summary>
        abstract public bool ShouldAddTotalMovedY { get; }

        abstract public Point? GetPosition(Model model);

        virtual public (Size size, Point position) CalculateNewSizeAndPosition(double deltaX, double deltaY)
        {
            _totalMovedX += deltaX;
            _totalMovedY += deltaY;

            var width = _originalSize!.Width + (ShouldAddTotalMovedX ? _totalMovedX : -_totalMovedX) / _diagram!.Zoom;
            var height = _originalSize.Height + (ShouldAddTotalMovedY ? _totalMovedY : -_totalMovedY) / _diagram!.Zoom;

            var positionX = _originalPosition!.X + (ShouldChangeXPositionOnResize ? _totalMovedX : 0) / _diagram!.Zoom;
            var positionY = _originalPosition.Y + (ShouldChangeYPositionOnResize ? _totalMovedY : 0) / _diagram!.Zoom;

            if (width < _nodeModel!.MinimumDimensions.Width)
            {
                width = _nodeModel.MinimumDimensions.Width;
                positionX = _nodeModel.Position.X;
            }
            if (height < _nodeModel.MinimumDimensions.Height)
            {
                height = _nodeModel.MinimumDimensions.Height;
                positionY = _nodeModel.Position.Y;
            }

            return (new Size(width, height), new Point(positionX, positionY));
        }

        virtual public void SetSizeAndPosition(Size size, Point position)
        {
            _nodeModel!.SetPosition(position.X, position.Y);
            _nodeModel.SetSize(size.Width, size.Height);
        }

        virtual public void OnResizeStart(Diagram diagram, Model model, PointerEventArgs e)
        {
            if (model is NodeModel nodeModel)
            {
                _lastClientX = e.ClientX;
                _lastClientY = e.ClientY;
                _originalPosition = new Point(nodeModel.Position.X, nodeModel.Position.Y);
                _originalSize = nodeModel.Size;
                _nodeModel = nodeModel;
                _diagram = diagram;
            }
        }

        virtual public void OnPointerMove(Model? model, PointerEventArgs e)
        {
            if (_originalSize is null || _originalPosition is null || _nodeModel is null || _diagram is null)
            {
                return;
            }

            var deltaX = (e.ClientX - _lastClientX!.Value);
            var deltaY = (e.ClientY - _lastClientY!.Value);

            _lastClientX = e.ClientX;
            _lastClientY = e.ClientY;

            var result = CalculateNewSizeAndPosition(deltaX, deltaY);
            SetSizeAndPosition(result.size, result.position);
        }

        virtual public void OnPanChanged(double deltaX, double deltaY)
        {
            if (_nodeModel is null) return;

            var result = CalculateNewSizeAndPosition(deltaX, deltaY);
            SetSizeAndPosition(result.size, result.position);
        }

        virtual public void OnResizeEnd(Model? model, PointerEventArgs args)
        {
            _nodeModel?.TriggerSizeChanged();
            _originalSize = null;
            _originalPosition = null;
            _nodeModel = null;
            _totalMovedX = 0;
            _totalMovedY = 0;
            _lastClientX = null;
            _lastClientY = null;
            _diagram = null;
        }
    }
}
