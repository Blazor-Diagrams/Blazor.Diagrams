using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models.Base;
using Microsoft.AspNetCore.Components.Web;

namespace Blazor.Diagrams.Core.Behaviors
{
    public class PanBehavior : Behavior
    {
        private Point? _initialPan;
        private double _lastClientX;
        private double _lastClientY;

        public PanBehavior(Diagram diagram) : base(diagram)
        {
            Diagram.MouseDown += OnMouseDown;
            Diagram.MouseMove += OnMouseMove;
            Diagram.MouseUp += OnMouseUp;
            Diagram.TouchStart += OnTouchStart;
            Diagram.TouchMove += OnTouchmove;
            Diagram.TouchEnd += OnTouchEnd;
        }

        private void OnTouchStart(Model model, TouchEventArgs e)
            => Start(model, e.ChangedTouches[0].ClientX, e.ChangedTouches[0].ClientY, e.ShiftKey);

        private void OnTouchmove(Model model, TouchEventArgs e)
            => Move(e.ChangedTouches[0].ClientX, e.ChangedTouches[0].ClientY);

        private void OnTouchEnd(Model model, TouchEventArgs e) => End();

        private void OnMouseDown(Model model, MouseEventArgs e)
        {
            if (e.Button != (int)MouseEventButton.Left)
                return;

            Start(model, e.ClientX, e.ClientY, e.ShiftKey);
        }

        private void OnMouseMove(Model model, MouseEventArgs e) => Move(e.ClientX, e.ClientY);

        private void OnMouseUp(Model model, MouseEventArgs e) => End();

        private void Start(Model model, double clientX, double clientY, bool shiftKey)
        {
            if (!Diagram.Options.AllowPanning || model != null || shiftKey)
                return;

            _initialPan = Diagram.Pan;
            _lastClientX = clientX;
            _lastClientY = clientY;
        }

        private void Move(double clientX, double clientY)
        {
            if (!Diagram.Options.AllowPanning || _initialPan == null)
                return;

            var deltaX = clientX - _lastClientX - (Diagram.Pan.X - _initialPan.X);
            var deltaY = clientY - _lastClientY - (Diagram.Pan.Y - _initialPan.Y);
            Diagram.UpdatePan(deltaX, deltaY);
        }

        private void End()
        {
            if (!Diagram.Options.AllowPanning)
                return;

            _initialPan = null;
        }

        public override void Dispose()
        {
            Diagram.MouseDown -= OnMouseDown;
            Diagram.MouseMove -= OnMouseMove;
            Diagram.MouseUp -= OnMouseUp;
            Diagram.TouchStart -= OnTouchStart;
            Diagram.TouchMove -= OnTouchmove;
            Diagram.TouchEnd -= OnTouchEnd;
        }
    }
}
