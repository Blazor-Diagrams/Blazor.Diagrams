using Blazor.Diagrams.Core;
using Blazor.Diagrams.Core.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;

namespace Blazor.Diagrams.Components
{
    public class DiagramCanvasComponent : ComponentBase, IDisposable
    {

        [CascadingParameter(Name = "DiagramManager")]
        public DiagramManager DiagramManager { get; set; }

        private bool _shouldReRender;

        protected override void OnInitialized()
        {
            base.OnInitialized();

            DiagramManager.LinkAdded += DiagramManager_LinkRelatedOperation;
            DiagramManager.LinkAttached += DiagramManager_LinkRelatedOperation;
            DiagramManager.LinkRemoved += DiagramManager_LinkRelatedOperation;
        }

        protected override bool ShouldRender()
        {
            if (_shouldReRender)
            {
                _shouldReRender = false;
                return true;
            }

            return false;
        }

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

        private void DiagramManager_LinkRelatedOperation(LinkModel link)
        {
            _shouldReRender = true;
            StateHasChanged();
        }

        public void Dispose()
        {
            DiagramManager.LinkAdded -= DiagramManager_LinkRelatedOperation;
            DiagramManager.LinkAttached -= DiagramManager_LinkRelatedOperation;
            DiagramManager.LinkRemoved -= DiagramManager_LinkRelatedOperation;
        }
    }
}
