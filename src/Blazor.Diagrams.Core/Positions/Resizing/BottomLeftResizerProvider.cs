using Blazor.Diagrams.Core.Events;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;

namespace Blazor.Diagrams.Core.Positions.Resizing
{
    public class BottomLeftResizerProvider : IResizerProvider
    {
        public string? Class => "bottomleft";

        private Size? _originalSize;
        private Point? _originalPosition;
        private Point? _originalMousePosition;
        private NodeModel? _nodeModel;
        private Diagram? _diagram;


        public Point? GetPosition(Model model)
        {
            if (model is NodeModel nodeModel && nodeModel.Size is not null)
            {
                return new Point(nodeModel.Position.X - 5, nodeModel.Position.Y + nodeModel.Size.Height + 5);
            }
            return null;
        }

        public void OnResizeStart(Diagram diagram, Model model, PointerEventArgs eventArgs)
        {
            if (model is NodeModel nodeModel)
            {
                _originalPosition = new Point(nodeModel.Position.X, nodeModel.Position.Y);
                _originalMousePosition = new Point(eventArgs.ClientX, eventArgs.ClientY);
                _originalSize = nodeModel.Size;
                _nodeModel = nodeModel;
                _diagram = diagram;
            }
        }

        public void OnPointerMove(Model? model, PointerEventArgs args)
        {
            if (_originalSize is null || _originalPosition is null || _originalMousePosition is null || _nodeModel is null || _diagram is null)
            {
                return;
            }

            var height = _originalSize.Height + (args.ClientY - _originalMousePosition.Y) / _diagram.Zoom;
            var width = _originalSize.Width - (args.ClientX - _originalMousePosition.X) / _diagram.Zoom;

            var positionX = _originalPosition.X + (args.ClientX - _originalMousePosition.X) / _diagram.Zoom;
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
            _originalSize = null;
            _originalPosition = null;
            _originalMousePosition = null;
            _nodeModel = null;
            _diagram = null;
        }

    }
}
