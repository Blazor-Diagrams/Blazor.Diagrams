using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;
using Microsoft.AspNetCore.Components.Web;

namespace Blazor.Diagrams.Core.Default
{
    public class DragNewLinkSubManager : DiagramSubManager
    {
        private LinkModel? _ongoingLink;

        public DragNewLinkSubManager(DiagramManager diagramManager) : base(diagramManager)
        {
            DiagramManager.MouseDown += DiagramManager_MouseDown;
            DiagramManager.MouseMove += DiagramManager_MouseMove;
            DiagramManager.MouseUp += DiagramManager_MouseUp;
        }

        private void DiagramManager_MouseDown(Model model, MouseEventArgs e)
        {
            if (!(model is PortModel port))
                return;

            _ongoingLink = DiagramManager.AddLink(port, null, onGoingPosition: new Point(
                e.ClientX - DiagramManager.Container.Left, e.ClientY - DiagramManager.Container.Top));
        }

        private void DiagramManager_MouseMove(Model model, MouseEventArgs e)
        {
            if (_ongoingLink == null || model != null)
                return;

            _ongoingLink.OnGoingPosition = new Point(e.ClientX - DiagramManager.Container.Left,
                e.ClientY - DiagramManager.Container.Top);
            _ongoingLink.Refresh();
        }

        private void DiagramManager_MouseUp(Model model, MouseEventArgs e)
        {
            if (_ongoingLink == null)
                return;

            if (!(model is PortModel port))
            {
                DiagramManager.RemoveLink(_ongoingLink);
                _ongoingLink = null;
                return;
            }

            DiagramManager.AttachLink(_ongoingLink, port);
            _ongoingLink = null;
        }

        public override void Dispose()
        {
            DiagramManager.MouseDown -= DiagramManager_MouseDown;
            DiagramManager.MouseMove -= DiagramManager_MouseMove;
            DiagramManager.MouseUp -= DiagramManager_MouseUp;
        }
    }
}
