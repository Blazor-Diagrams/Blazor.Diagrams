using Blazor.Diagrams.Core.Events;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;

namespace Blazor.Diagrams.Core.Positions.Resizing
{
    public class BottomRightResizerProvider : IResizerProvider
    {
        public string? Class => "bottomright";

        private Size? _originalSize;
        private Point? _originalMousePosition;
        private NodeModel? _nodeModel;
        private Diagram? _diagram;

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
                _originalSize = nodeModel.Size;
                _nodeModel = nodeModel;
                _diagram = diagram;
            }
        }

        public void OnPointerMove(Model? model, PointerEventArgs args)
        {
            if (_originalSize is null || _originalMousePosition is null || _nodeModel is null || _diagram is null)
            {
                return;
            }

            var height = _originalSize.Height + (args.ClientY - _originalMousePosition.Y) / _diagram.Zoom;
            var width = _originalSize.Width + (args.ClientX - _originalMousePosition.X) / _diagram.Zoom;

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
            _originalSize = null;
            _originalMousePosition = null;
            _nodeModel = null;
            _diagram = null;
        }

    }
}
