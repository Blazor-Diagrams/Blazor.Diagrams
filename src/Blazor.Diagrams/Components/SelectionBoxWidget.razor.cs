using Blazor.Diagrams.Core;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models.Base;
using Blazor.Diagrams.Core.Events;
using Microsoft.AspNetCore.Components;
using System;

namespace Blazor.Diagrams.Components
{
    public partial class SelectionBoxWidget : IDisposable
    {
        private Point _initialClientPoint;
        private Point _selectionBoxTopLeft;
        private Size _selectionBoxSize;

        [CascadingParameter]
        public Diagram Diagram { get; set; }

        [Parameter]
        public string Background { get; set; } = "rgb(110 159 212 / 25%);";

        protected override void OnInitialized()
        {
            Diagram.PointerDown += OnPointerDown;
            Diagram.PointerMove += OnPointerMove;
            Diagram.PointerUp += OnPointerUp;
        }

        private string GenerateStyle()
            => FormattableString.Invariant($"position: absolute; background: {Background}; top: {_selectionBoxTopLeft.Y}px; left: {_selectionBoxTopLeft.X}px; width: {_selectionBoxSize.Width}px; height: {_selectionBoxSize.Height}px;");

        private void OnPointerDown(Model model, MouseEventArgs e)
        {
            if (model != null || !e.ShiftKey)
                return;

            _initialClientPoint = new Point(e.ClientX, e.ClientY);
        }

        private void OnPointerMove(Model model, MouseEventArgs e)
        {
            if (_initialClientPoint == null)
                return;

            SetSelectionBoxInformation(e);

            var start = Diagram.GetRelativeMousePoint(_initialClientPoint.X, _initialClientPoint.Y);
            var end = Diagram.GetRelativeMousePoint(e.ClientX, e.ClientY);
            (var sX, var sY) = (Math.Min(start.X, end.X), Math.Min(start.Y, end.Y));
            (var eX, var eY) = (Math.Max(start.X, end.X), Math.Max(start.Y, end.Y));
            var bounds = new Rectangle(sX, sY, eX, eY);
            
            foreach (var node in Diagram.Nodes)
            {
                if (bounds.Overlap(node.GetBounds()))
                {
                    Diagram.SelectModel(node, false);
                }
                else if (node.Selected)
                {
                    Diagram.UnselectModel(node);
                }
            }

            InvokeAsync(StateHasChanged);
        }

        private void SetSelectionBoxInformation(MouseEventArgs e)
        {
            var start = Diagram.GetRelativePoint(_initialClientPoint.X, _initialClientPoint.Y);
            var end = Diagram.GetRelativePoint(e.ClientX, e.ClientY);
            (var sX, var sY) = (Math.Min(start.X, end.X), Math.Min(start.Y, end.Y));
            (var eX, var eY) = (Math.Max(start.X, end.X), Math.Max(start.Y, end.Y));
            _selectionBoxTopLeft = new Point(sX, sY);
            _selectionBoxSize = new Size(eX - sX, eY - sY);
        }

        private void OnPointerUp(Model model, MouseEventArgs e)
        {
            _initialClientPoint = null;
            _selectionBoxTopLeft = null;
            _selectionBoxSize = null;
            InvokeAsync(StateHasChanged);
        }

        public void Dispose()
        {
            Diagram.PointerDown -= OnPointerDown;
            Diagram.PointerMove -= OnPointerMove;
            Diagram.PointerUp -= OnPointerUp;
        }
    }
}
