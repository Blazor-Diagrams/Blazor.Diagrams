using Blazor.Diagrams.Core.Models.Base;
using Microsoft.AspNetCore.Components.Web;

namespace Blazor.Diagrams.Core.Behaviors
{
    public class SelectionBehavior : Behavior
    {
        public SelectionBehavior(Diagram diagram) : base(diagram)
        {
            Diagram.MouseDown += Diagram_MouseDown;
        }

        private void Diagram_MouseDown(Model model, MouseEventArgs e)
        {
            if (model == null)
            {
                Diagram.UnselectAll();
            }
            else if (model is SelectableModel sm)
            {
                if (e.CtrlKey && sm.Selected)
                {
                    Diagram.UnselectModel(sm);
                }
                else if (!sm.Selected)
                {
                    Diagram.SelectModel(sm, !e.CtrlKey || !Diagram.Options.AllowMultiSelection);
                }
            }
        }

        public override void Dispose()
        {
            Diagram.MouseDown -= Diagram_MouseDown;
        }
    }
}
