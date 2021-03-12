using Blazor.Diagrams.Core;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models.Base;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
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
            Diagram.MouseDown += OnMouseDown;
            Diagram.MouseMove += OnMouseMove;
            Diagram.MouseUp += OnMouseUp;
        }

        private string GenerateStyle()
            => FormattableString.Invariant($"position: absolute; background: {Background}; top: {_selectionBoxTopLeft.Y}px; left: {_selectionBoxTopLeft.X}px; width: {_selectionBoxSize.Width}px; height: {_selectionBoxSize.Height}px;");

        private void OnMouseDown(Model model, MouseEventArgs e)
        {
            if (model != null || !e.ShiftKey)
                return;

            _initialClientPoint = new Point(e.ClientX, e.ClientY);
        }

        private void OnMouseMove(Model model, MouseEventArgs e)
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

            StateHasChanged();
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

        private void OnMouseUp(Model model, MouseEventArgs e)
        {
            _initialClientPoint = null;
            _selectionBoxTopLeft = null;
            _selectionBoxSize = null;
            StateHasChanged();
        }

        public void Dispose()
        {
            Diagram.MouseDown -= OnMouseDown;
            Diagram.MouseMove -= OnMouseMove;
            Diagram.MouseUp -= OnMouseUp;
        }
    }
}
