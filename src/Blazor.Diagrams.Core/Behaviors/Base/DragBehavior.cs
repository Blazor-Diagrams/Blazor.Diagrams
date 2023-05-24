using Blazor.Diagrams.Core.Events;
using Blazor.Diagrams.Core.Models.Base;

namespace Blazor.Diagrams.Core.Behaviors.Base
{
    public abstract class DragBehavior : Behavior
    {
        public DragBehavior(Diagram diagram)
            : base(diagram)
        {
            Diagram.PointerDown += OnPointerDown;
            Diagram.PointerMove += OnPointerMove;
            Diagram.PointerUp += OnPointerUp;
        }

        protected abstract void OnPointerDown(Model? model, PointerEventArgs e);

        protected abstract void OnPointerMove(Model? model, PointerEventArgs e);

        protected abstract void OnPointerUp(Model? model, PointerEventArgs e);

        public virtual bool IsBehaviorEnabled(PointerEventArgs e)
        {
            if (e.AltKey && !e.CtrlKey && !e.ShiftKey
                && Diagram.Options.Behaviors.DiagramAltDragBehavior is not null)
            {
                return this == Diagram.Options.Behaviors.DiagramAltDragBehavior;
            }
            else if (!e.AltKey && e.CtrlKey && !e.ShiftKey
                && Diagram.Options.Behaviors.DiagramCtrlDragBehavior is not null)
            {
                return this == Diagram.Options.Behaviors.DiagramCtrlDragBehavior;
            }
            else if (!e.AltKey && !e.CtrlKey && e.ShiftKey
                && Diagram.Options.Behaviors.DiagramShiftDragBehavior is not null)
            {
                return this == Diagram.Options.Behaviors.DiagramShiftDragBehavior;
            }
            return this == Diagram.Options.Behaviors.DiagramDragBehavior;
        }

        public override void Dispose()
        {
            Diagram.PointerDown -= OnPointerDown;
            Diagram.PointerMove -= OnPointerMove;
            Diagram.PointerUp -= OnPointerUp;
        }
    }
}
