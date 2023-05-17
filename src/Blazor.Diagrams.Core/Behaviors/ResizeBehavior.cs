using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models.Base;
using Blazor.Diagrams.Core.Events;
using Blazor.Diagrams.Core.Routers;
using Blazor.Diagrams.Core.Models;

namespace Blazor.Diagrams.Core.Behaviors
{
    public class ResizeBehavior : Behavior
    {
        private Size _originalSize = null!;
        private Point _originalPosition = null!;
        private Point _originalMousePosition = null!;
        private ResizerModel? _resizer = null;

        public ResizeBehavior(Diagram diagram) : base(diagram)
        {
            Diagram.PointerDown += OnPointerDown;
            Diagram.PointerMove += OnPointerMove;
            Diagram.PointerUp += OnPointerUp;
        }

        private void OnPointerDown(Model? model, PointerEventArgs e)
        {
            if (model is not ResizerModel)
                return;

            _resizer = (ResizerModel)model;
            _originalPosition = new Point(_resizer.Parent.Position.X, _resizer.Parent.Position.Y);
            _originalMousePosition = new Point(e.ClientX, e.ClientY);
            _originalSize = _resizer.Parent.Size!;
        }
        
        private void OnPointerMove(Model? model, PointerEventArgs args)
        {
            if (_resizer == null)
                return;
            Resize(_resizer.Alignment, _resizer.Parent, args);
        }

        private void OnPointerUp(Model? model, PointerEventArgs args)
        {
            _resizer = null;
        }

        void Resize(ResizerPosition resizerAlignment, NodeModel model, PointerEventArgs args)
        {
            var width = _originalSize.Width;
            var height = _originalSize.Height;
            var positionX = model.Position.X;
            var positionY = model.Position.Y;

            if (resizerAlignment == ResizerPosition.TopLeft)
            {
                height = _originalSize.Height - (args.ClientY - _originalMousePosition.Y);
                width = _originalSize.Width - (args.ClientX - _originalMousePosition.X);

                positionX = _originalPosition.X + (args.ClientX - _originalMousePosition.X);
                positionY = _originalPosition.Y + (args.ClientY - _originalMousePosition.Y);
            }
            else if (resizerAlignment == ResizerPosition.TopRight)
            {
                height = _originalSize.Height - (args.ClientY - _originalMousePosition.Y);
                width = _originalSize.Width + (args.ClientX - _originalMousePosition.X);

                positionX = _originalPosition.X;
                positionY = _originalPosition.Y + (args.ClientY - _originalMousePosition.Y);
            }
            else if (resizerAlignment == ResizerPosition.BottomLeft)
            {
                height = _originalSize.Height + (args.ClientY - _originalMousePosition.Y);
                width = _originalSize.Width - (args.ClientX - _originalMousePosition.X);

                positionX = _originalPosition.X + (args.ClientX - _originalMousePosition.X);
                positionY = _originalPosition.Y;
            }
            else if (resizerAlignment == ResizerPosition.BottomRight)
            {
                height = _originalSize.Height + (args.ClientY - _originalMousePosition.Y);
                width = _originalSize.Width + (args.ClientX - _originalMousePosition.X);
            }
            else if (resizerAlignment == ResizerPosition.Top)
            {
                height = _originalSize.Height - (args.ClientY - _originalMousePosition.Y);

                positionY = _originalPosition.Y + (args.ClientY - _originalMousePosition.Y);
            }
            else if (resizerAlignment == ResizerPosition.Right)
            {
                width = _originalSize.Width + (args.ClientX - _originalMousePosition.X);
            }
            else if (resizerAlignment == ResizerPosition.Left)
            {
                width = _originalSize.Width - (args.ClientX - _originalMousePosition.X);

                positionX = _originalPosition.X + (args.ClientX - _originalMousePosition.X);
            }
            else if (resizerAlignment == ResizerPosition.Bottom)
            {
                height = _originalSize.Height + (args.ClientY - _originalMousePosition.Y);
            }

            if (width < model.MinimumDimensions.Width)
            {
                width = model.MinimumDimensions.Width;
                positionX = model.Position.X;
            }
            if (height < model.MinimumDimensions.Height)
            {
                height = model.MinimumDimensions.Height;
                positionY = model.Position.Y;
            }

            model.SetPosition(positionX, positionY);
            model.Size = new Size(width, height);

            model.Refresh();
        }

        public override void Dispose()
        {
            Diagram.PointerDown -= OnPointerDown;
            Diagram.PointerMove -= OnPointerMove;
            Diagram.PointerUp -= OnPointerUp;
        }
    }
}
