using Blazor.Diagrams.Core;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Blazor.Diagrams.Components
{
    public class DiagramCanvasComponent : ComponentBase
    {

        [CascadingParameter(Name = "DiagramManager")]
        public DiagramManager DiagramManager { get; set; }

        protected override bool ShouldRender() => false;

        protected void OnMouseDown(MouseEventArgs e)
        {
            DiagramManager.OnMouseDown(null, e);
        }

        protected void OnMouseMove(MouseEventArgs e)
        {
            DiagramManager.OnMouseMove(null, e);
        }

        protected void OnMouseUp(MouseEventArgs e)
        {
            DiagramManager.OnMouseUp(null, e);
        }
    }
}
