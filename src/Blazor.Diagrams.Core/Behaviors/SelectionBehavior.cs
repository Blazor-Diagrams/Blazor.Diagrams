using Blazor.Diagrams.Core.Models.Base;
using Blazor.Diagrams.Core.Events;

namespace Blazor.Diagrams.Core.Behaviors
{
    public class SelectionBehavior : Behavior
    {
        public SelectionBehavior(DiagramBase diagram) : base(diagram)
        {
            Diagram.PointerDown += OnPointerDown;
        }

        private void OnPointerDown(Model? model, PointerEventArgs e) => Process(model, e.CtrlKey);

        private void Process(Model? model, bool ctrlKey)
        {
            switch (model)
            {
                case null:
                    Diagram.UnselectAll();
                    break;
                case SelectableModel sm when ctrlKey && sm.Selected:
                    Diagram.UnselectModel(sm);
                    break;
                case SelectableModel sm:
                {
                    if (!sm.Selected)
                    {
                        Diagram.SelectModel(sm, !ctrlKey || !Diagram.Options.AllowMultiSelection);
                    }

                    break;
                }
            }
        }

        public override void Dispose()
        {
            Diagram.PointerDown -= OnPointerDown;
        }
    }
}
