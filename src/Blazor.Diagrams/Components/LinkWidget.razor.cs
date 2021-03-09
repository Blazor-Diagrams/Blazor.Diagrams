using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core;
using Microsoft.AspNetCore.Components;
using Blazor.Diagrams.Core.Models.Core;
using Microsoft.AspNetCore.Components.Web;

namespace Blazor.Diagrams.Components
{
    public partial class LinkWidget
    {
        [CascadingParameter]
        public Diagram Diagram { get; set; }

        [Parameter]
        public LinkModel Link { get; set; }

        private void OnMouseDown(MouseEventArgs e, int index)
        {
            if (!Link.Segmentable)
                return;

            var rPt = Diagram.GetRelativeMousePoint(e.ClientX, e.ClientY);
            var vertex = new LinkVertexModel(Link, rPt);
            Link.Vertices.Insert(index, vertex);
            Diagram.OnMouseDown(vertex, e);
        }
    }
}
