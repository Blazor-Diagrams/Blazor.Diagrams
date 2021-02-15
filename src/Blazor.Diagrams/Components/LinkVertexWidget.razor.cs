using Blazor.Diagrams.Core;
using Blazor.Diagrams.Core.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;

namespace Blazor.Diagrams.Components
{
    public partial class LinkVertexWidget : IDisposable
    {
        private bool _shouldRender = true;

        [CascadingParameter(Name = "DiagramManager")] public DiagramManager DiagramManager { get; set; }
        [Parameter] public LinkVertexModel Vertex { get; set; }
        [Parameter] public string Color { get; set; }
        [Parameter] public string SelectedColor { get; set; }

        private string ColorToUse => Vertex.Selected ? SelectedColor : Color;

        public void Dispose()
        {
            Vertex.Changed -= OnVertexChanged;
        }

        protected override void OnInitialized()
        {
            Vertex.Changed += OnVertexChanged;
        }

        protected override bool ShouldRender()
        {
            if (_shouldRender)
            {
                _shouldRender = false;
                return true;
            }

            return false;
        }

        private void OnVertexChanged()
        {
            _shouldRender = true;
            StateHasChanged();
        }

        private void OnMouseDown(MouseEventArgs e) => DiagramManager.OnMouseDown(Vertex, e);

        private void OnMouseUp(MouseEventArgs e) => DiagramManager.OnMouseUp(Vertex, e);

        private void OnDoubleClick(MouseEventArgs e)
        {
            Vertex.Parent.Vertices.Remove(Vertex);
            Vertex.Parent.Refresh();
        }
    }
}
