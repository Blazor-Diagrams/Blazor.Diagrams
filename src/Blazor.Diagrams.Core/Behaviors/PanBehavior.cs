using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models.Base;
using Blazor.Diagrams.Core.Events;
using Blazor.Diagrams.Core.Options;

namespace Blazor.Diagrams.Core.Behaviors
{
    public class PanBehavior : Behavior
    {
        private Point? _initialPan;
        private double _lastClientX;
        private double _lastClientY;

        public PanBehavior(Diagram diagram) : base(diagram)
        {
            Diagram.PointerDown += OnPointerDown;
            Diagram.PointerMove += OnPointerMove;
            Diagram.PointerUp += OnPointerUp;
        }

        private void OnPointerDown(Model? model, PointerEventArgs e)
        {
            if (e.Button != (int)MouseEventButton.Left || model != null || !Diagram.Options.AllowPanning || !Diagram.IsBehaviorEnabled(e, DiagramDragBehavior.Pan))
                return;

            _initialPan = Diagram.Pan;
            _lastClientX = e.ClientX;
            _lastClientY = e.ClientY;
        }

        private void OnPointerMove(Model? model, PointerEventArgs e) => Move(e.ClientX, e.ClientY);

        private void OnPointerUp(Model? model, PointerEventArgs e) => End();

        private void Move(double clientX, double clientY)
        {
            if (_initialPan == null)
                return;

            var deltaX = clientX - _lastClientX - (Diagram.Pan.X - _initialPan.X);
            var deltaY = clientY - _lastClientY - (Diagram.Pan.Y - _initialPan.Y);
            Diagram.UpdatePan(deltaX, deltaY);
        }

        private void End()
        {
            _initialPan = null;
        }

        public override void Dispose()
        {
            Diagram.PointerDown -= OnPointerDown;
            Diagram.PointerMove -= OnPointerMove;
            Diagram.PointerUp -= OnPointerUp;
        }
    }
}
