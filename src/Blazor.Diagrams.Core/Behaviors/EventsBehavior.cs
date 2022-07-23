using Blazor.Diagrams.Core.Models.Base;
using Blazor.Diagrams.Core.Events;
using System;
using System.Diagnostics;

namespace Blazor.Diagrams.Core.Behaviors
{
    public class EventsBehavior : Behavior
    {
        private readonly Stopwatch _mouseClickSw;
        private Model? _model;
        private bool _captureMouseMove;
        private int _mouseMovedCount;

        public EventsBehavior(DiagramBase diagram) : base(diagram)
        {
            _mouseClickSw = new Stopwatch();

            Diagram.MouseDown += OnMouseDown;
            Diagram.MouseMove += OnMouseMove;
            Diagram.MouseUp += OnMouseUp;
            Diagram.MouseClick += OnMouseClick;
        }

        private void OnMouseClick(Model? model, MouseEventArgs e)
        {
            if (_mouseClickSw.IsRunning && _mouseClickSw.ElapsedMilliseconds <= 500)
            {
                Diagram.OnMouseDoubleClick(model, e);
            }

            _mouseClickSw.Restart();
        }

        private void OnMouseDown(Model? model, MouseEventArgs e)
        {
            _captureMouseMove = true;
            _mouseMovedCount = 0;
            _model = model;
        }

        private void OnMouseMove(Model? model, MouseEventArgs e)
        {
            if (!_captureMouseMove)
                return;

            _mouseMovedCount++;
        }

        private void OnMouseUp(Model? model, MouseEventArgs e)
        {
            if (!_captureMouseMove) return; // Only set by OnMouseDown
            _captureMouseMove = false;
            if (_mouseMovedCount > 0) return;

            if (_model == model)
            {
                Diagram.OnMouseClick(model, e);
                _model = null;
            }
        }

        public override void Dispose()
        {
            Diagram.MouseDown -= OnMouseDown;
            Diagram.MouseMove -= OnMouseMove;
            Diagram.MouseUp -= OnMouseUp;
            Diagram.MouseClick -= OnMouseClick;
            _model = null;
        }
    }
}
