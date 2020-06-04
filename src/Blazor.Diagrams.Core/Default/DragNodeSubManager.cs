using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;
using Microsoft.AspNetCore.Components.Web;

namespace Blazor.Diagrams.Core.Default
{
    public class DragNodeSubManager : DiagramSubManager
    {
        private NodeModel? _draggedNode;

        public DragNodeSubManager(DiagramManager diagramManager) : base(diagramManager)
        {
            DiagramManager.MouseDown += DiagramManager_MouseDown;
            DiagramManager.MouseMove += DiagramManager_MouseMove;
            DiagramManager.MouseUp += DiagramManager_MouseUp;
        }

        private void DiagramManager_MouseDown(Model model, MouseEventArgs e)
        {
            if (!(model is NodeModel node))
                return;

            _draggedNode = node;
        }

        private void DiagramManager_MouseMove(Model model, MouseEventArgs e)
        {
            if (_draggedNode == null)
                return;

            _draggedNode.UpdatePosition(e.ClientX, e.ClientY);
            _draggedNode.RefreshAll();
        }

        private void DiagramManager_MouseUp(Model model, MouseEventArgs e)
        {
            _draggedNode = null;
        }

        public override void Dispose()
        {
            DiagramManager.MouseDown -= DiagramManager_MouseDown;
            DiagramManager.MouseMove -= DiagramManager_MouseMove;
            DiagramManager.MouseUp -= DiagramManager_MouseUp;
        }
    }
}
