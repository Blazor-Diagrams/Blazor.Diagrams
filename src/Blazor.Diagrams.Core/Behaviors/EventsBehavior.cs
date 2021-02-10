using Blazor.Diagrams.Core.Models.Base;
using Microsoft.AspNetCore.Components.Web;
using System;

namespace Blazor.Diagrams.Core.Behaviors
{
    public class EventsBehavior : Behavior
    {
        private bool _captureMouseMove;
        private int _mouseMovedCount;

        public EventsBehavior(DiagramManager diagramManager) : base(diagramManager)
        {
            DiagramManager.MouseDown += OnMouseDown;
            DiagramManager.MouseMove += OnMouseMove;
            DiagramManager.MouseUp += OnMouseUp;
        }

        private void OnMouseDown(Model model, MouseEventArgs e)
        {
            _captureMouseMove = true;
        }

        private void OnMouseMove(Model model, MouseEventArgs e)
        {
            if (!_captureMouseMove)
                return;

            _mouseMovedCount++;
        }

        private void OnMouseUp(Model model, MouseEventArgs e)
        {
            _captureMouseMove = false;
            if (_mouseMovedCount > 0)
            {
                _mouseMovedCount = 0;
                return;
            }

            DiagramManager.OnMouseClick(model, e);
        }

        public override void Dispose()
        {
            DiagramManager.MouseDown -= OnMouseDown;
            DiagramManager.MouseMove -= OnMouseMove;
            DiagramManager.MouseUp -= OnMouseUp;
        }
    }
}
