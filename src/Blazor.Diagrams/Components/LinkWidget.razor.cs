using Blazor.Diagrams.Core.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Blazor.Diagrams.Extensions;

namespace Blazor.Diagrams.Components
{
    public partial class LinkWidget
    {
        [CascadingParameter]
        public Diagram Diagram { get; set; } = null!;

        [Parameter]
        public LinkModel Link { get; set; } = null!;

        private void OnMouseDown(MouseEventArgs e, int index)
        {
            if (!Link.Segmentable)
                return;

            var vertex = CreateVertex(e.ClientX, e.ClientY, index);
            Diagram.OnMouseDown(vertex, e.ToCore());
        }

        private void OnTouchStart(TouchEventArgs e, int index)
        {
            if (!Link.Segmentable)
                return;

            var vertex = CreateVertex(e.ChangedTouches[0].ClientX, e.ChangedTouches[0].ClientY, index);
            Diagram.OnTouchStart(vertex, e.ToCore());
        }

        private LinkVertexModel CreateVertex(double clientX, double clientY, int index)
        {
            var rPt = Diagram.GetRelativeMousePoint(clientX, clientY);
            var vertex = new LinkVertexModel(Link, rPt);
            Link.Vertices.Insert(index, vertex);
            return vertex;
        }
    }
}
