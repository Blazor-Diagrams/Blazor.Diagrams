using Blazor.Diagrams.Core.Models.Base;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Diagnostics;

namespace Blazor.Diagrams.Core.Behaviors
{
    public class EventsBehavior : Behavior
    {
        private readonly Stopwatch _mouseClickSw;
        private bool _captureMouseMove;
        private int _mouseMovedCount;

        public EventsBehavior(Diagram diagram) : base(diagram)
        {
            _mouseClickSw = new Stopwatch();

            Diagram.MouseDown += OnMouseDown;
            Diagram.MouseMove += OnMouseMove;
            Diagram.MouseUp += OnMouseUp;
            Diagram.MouseClick += OnMouseClick;
        }

        private void OnMouseClick(Model model, MouseEventArgs e)
        {
            if (_mouseClickSw.IsRunning && _mouseClickSw.ElapsedMilliseconds <= 500)
            {
                Diagram.OnMouseDoubleClick(model, e);
            }

            _mouseClickSw.Restart();
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

            Diagram.OnMouseClick(model, e);
        }

        public override void Dispose()
        {
            Diagram.MouseDown -= OnMouseDown;
            Diagram.MouseMove -= OnMouseMove;
            Diagram.MouseUp -= OnMouseUp;
        }
    }
}
