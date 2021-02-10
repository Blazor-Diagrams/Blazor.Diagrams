using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core;
using Microsoft.AspNetCore.Components;
using Blazor.Diagrams.Core.Models.Core;

namespace Blazor.Diagrams.Components
{
    public partial class LinkWidget
    {

        [CascadingParameter(Name = "DiagramManager")]
        public DiagramManager DiagramManager { get; set; }

        [Parameter]
        public LinkModel Link { get; set; }

        private Point GetPositionBasedOnAlignment(PortModel port, LinkMarker marker)
        {
            if (marker == null)
                return port.MiddlePosition;

            var pt = port.Position;
            switch (port.Alignment)
            {
                case PortAlignment.Top:
                    return new Point(pt.X + port.Size.Width / 2, pt.Y);
                case PortAlignment.TopRight:
                    return new Point(pt.X + port.Size.Width, pt.Y);
                case PortAlignment.Right:
                    return new Point(pt.X + port.Size.Width, pt.Y + port.Size.Height / 2);
                case PortAlignment.BottomRight:
                    return new Point(pt.X + port.Size.Width, pt.Y + port.Size.Height);
                case PortAlignment.Bottom:
                    return new Point(pt.X + port.Size.Width / 2, pt.Y + port.Size.Height);
                case PortAlignment.BottomLeft:
                    return new Point(pt.X, pt.Y + port.Size.Height);
                case PortAlignment.Left:
                    return new Point(pt.X, pt.Y + port.Size.Height / 2);
                default:
                    return pt;
            }
        }
    }
}
