using Blazor.Diagrams.Core.Models;
using Microsoft.AspNetCore.Components.Web;
using System;

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

            _ongoingLink = DiagramManager.AddLink(port);
        }

        private void DiagramManager_MouseMove(Model model, MouseEventArgs e)
        {
            if (_ongoingLink == null)
                return;

            _ongoingLink.OnGoingPosition = new Point(e.ClientX, e.ClientY);
            _ongoingLink.Refresh();
            Console.WriteLine("Refresing link " + _ongoingLink.Id);
        }

        private void DiagramManager_MouseUp(Model model, MouseEventArgs e)
        {
            if (_ongoingLink == null)
                return;

            if (!(model is PortModel port))
            {
                DiagramManager.RemoveLink(_ongoingLink);
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
