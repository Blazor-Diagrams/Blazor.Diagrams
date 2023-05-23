using Blazor.Diagrams.Core.Behaviors;
using Blazor.Diagrams.Core.Behaviors.Base;
using System;
using System.Runtime.CompilerServices;

namespace Blazor.Diagrams.Core.Options
{
    public class DiagramBehaviorOptions
    {
        public Type? DiagramWheelBehavior
        {
            get => diagramWheelBehavior;
            set => UpdateBehaviorOption<WheelBehavior>(ref diagramWheelBehavior, value);
        }
        Type? diagramWheelBehavior;

        public Type? DiagramAltWheelBehavior
        {
            get => diagramAltWheelBehavior;
            set => UpdateBehaviorOption<WheelBehavior>(ref diagramAltWheelBehavior, value);
        }
        Type? diagramAltWheelBehavior;

        public Type? DiagramCtrlWheelBehavior
        {
            get => diagramCtrlWheelBehavior;
            set => UpdateBehaviorOption<WheelBehavior>(ref diagramCtrlWheelBehavior, value);
        }
        Type? diagramCtrlWheelBehavior;

        public Type? DiagramShiftWheelBehavior
        {
            get => diagramShiftWheelBehavior;
            set => UpdateBehaviorOption<WheelBehavior>(ref diagramShiftWheelBehavior, value);
        }
        Type? diagramShiftWheelBehavior;

        public Type? DiagramDragBehavior
        {
            get => diagramDragBehavior;
            set => UpdateBehaviorOption<DragBehavior>(ref diagramDragBehavior, value);
        }
        Type? diagramDragBehavior;

        public Type? DiagramAltDragBehavior
        {
            get => diagramAltDragBehavior;
            set => UpdateBehaviorOption<DragBehavior>(ref diagramAltDragBehavior, value);
        }
        Type? diagramAltDragBehavior;

        public Type? DiagramCtrlDragBehavior
        {
            get => diagramCtrlDragBehavior;
            set => UpdateBehaviorOption<DragBehavior>(ref diagramCtrlDragBehavior, value);
        }
        Type? diagramCtrlDragBehavior;

        public Type? DiagramShiftDragBehavior
        {
            get => diagramShiftDragBehavior;
            set => UpdateBehaviorOption<DragBehavior>(ref diagramShiftDragBehavior, value);
        }
        Type? diagramShiftDragBehavior;

        void UpdateBehaviorOption<T>(ref Type? property, Type? value, [CallerMemberName] string propertyName = "") where T : Behavior
        {
            if (value is not null && !value.IsSubclassOf(typeof(T)))
            {
                throw new InvalidOperationException($"{propertyName} must be a type of {typeof(T).Name}");
            }
            property = value;
        }
    }
}
