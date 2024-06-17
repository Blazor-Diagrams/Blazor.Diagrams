using Blazor.Diagrams.Core.Events;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;

namespace Blazor.Diagrams.Core.Positions.Resizing
{
    public abstract class ResizerProvider : IPositionProvider
    {
        abstract public string? Class { get; }

        protected Size? OriginalSize { get; set; }
        protected Point? OriginalPosition { get; set; }
        protected double? LastClientX { get; set; }
        protected double? LastClientY { get; set; }
        protected NodeModel? NodeModel { get; set; }
        protected Diagram? Diagram { get; set; }
        private double _totalMovedX = 0;
        private double _totalMovedY = 0;

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

            var width = OriginalSize!.Width + (ShouldAddTotalMovedX ? _totalMovedX : -_totalMovedX) / Diagram!.Zoom;
            var height = OriginalSize.Height + (ShouldAddTotalMovedY ? _totalMovedY : -_totalMovedY) / Diagram!.Zoom;

            var positionX = OriginalPosition!.X + (ShouldChangeXPositionOnResize ? _totalMovedX : 0) / Diagram!.Zoom;
            var positionY = OriginalPosition.Y + (ShouldChangeYPositionOnResize ? _totalMovedY : 0) / Diagram!.Zoom;

            if (width < NodeModel!.MinimumDimensions.Width)
            {
                width = NodeModel.MinimumDimensions.Width;

                if (ShouldChangeXPositionOnResize)
                {
                    positionX = OriginalPosition.X + OriginalSize.Width - NodeModel.MinimumDimensions.Width;
                }
            }
            if (height < NodeModel.MinimumDimensions.Height)
            {
                height = NodeModel.MinimumDimensions.Height;

                if (ShouldChangeYPositionOnResize)
                {
                    positionY = OriginalPosition.Y + OriginalSize.Height - NodeModel.MinimumDimensions.Height;
                }
            }

            return (new Size(width, height), new Point(positionX, positionY));
        }

        virtual public void SetSizeAndPosition(Size size, Point position)
        {
            NodeModel!.SetPosition(position.X, position.Y);
            NodeModel.SetSize(size.Width, size.Height);
        }

        virtual public void OnResizeStart(Diagram diagram, Model model, PointerEventArgs e)
        {
            if (model is NodeModel nodeModel)
            {
                LastClientX = e.ClientX;
                LastClientY = e.ClientY;
                OriginalPosition = new Point(nodeModel.Position.X, nodeModel.Position.Y);
                OriginalSize = nodeModel.Size;
                this.NodeModel = nodeModel;
                Diagram = diagram;
            }
        }

        virtual public void OnPointerMove(Model? model, PointerEventArgs e)
        {
            if (OriginalSize is null || OriginalPosition is null || NodeModel is null || Diagram is null)
            {
                return;
            }

            var deltaX = (e.ClientX - LastClientX!.Value);
            var deltaY = (e.ClientY - LastClientY!.Value);

            LastClientX = e.ClientX;
            LastClientY = e.ClientY;

            var result = CalculateNewSizeAndPosition(deltaX, deltaY);
            SetSizeAndPosition(result.size, result.position);
        }

        virtual public void OnPanChanged(double deltaX, double deltaY)
        {
            if (NodeModel is null) return;

            var result = CalculateNewSizeAndPosition(deltaX, deltaY);
            SetSizeAndPosition(result.size, result.position);
        }

        virtual public void OnResizeEnd(Model? model, PointerEventArgs args)
        {
            NodeModel?.TriggerSizeChanged();
            OriginalSize = null;
            OriginalPosition = null;
            NodeModel = null;
            _totalMovedX = 0;
            _totalMovedY = 0;
            LastClientX = null;
            LastClientY = null;
            Diagram = null;
        }
    }
}
