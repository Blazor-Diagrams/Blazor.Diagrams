using Blazor.Diagrams.Core.Models.Base;
using Microsoft.AspNetCore.Components.Web;

namespace Blazor.Diagrams.Core.Behaviors
{
    public class SelectionBehavior : Behavior
    {
        public SelectionBehavior(Diagram diagram) : base(diagram)
        {
            Diagram.MouseDown += OnMouseDown;
            Diagram.TouchStart += OnTouchStart;
        }

        private void OnTouchStart(Model model, TouchEventArgs e) => Process(model, e.CtrlKey);

        private void OnMouseDown(Model model, MouseEventArgs e) => Process(model, e.CtrlKey);

        private void Process(Model model, bool ctrlKey)
        {
            if (model == null)
            {
                Diagram.UnselectAll();
            }
            else if (model is SelectableModel sm)
            {
                if (ctrlKey && sm.Selected)
                {
                    Diagram.UnselectModel(sm);
                }
                else if (!sm.Selected)
                {
                    Diagram.SelectModel(sm, !ctrlKey || !Diagram.Options.AllowMultiSelection);
                }
            }
        }

        public override void Dispose()
        {
            Diagram.MouseDown -= OnMouseDown;
            Diagram.TouchStart -= OnTouchStart;
        }
    }
}
