using Blazor.Diagrams.Core.Geometry;
using Microsoft.AspNetCore.Components.Web;
using System;

namespace Blazor.Diagrams.Core.Behaviors
{
    public class ZoomBehavior : Behavior
    {
        public ZoomBehavior(Diagram diagram) : base(diagram)
        {
            Diagram.Wheel += Diagram_Wheel;
        }

        private void Diagram_Wheel(WheelEventArgs e)
        {
            if (!Diagram.Options.Zoom.Enabled)
                return;

            var scale = Math.Clamp(Diagram.Options.Zoom.ScaleFactor, 1.01, 2);

            var oldZoom = Diagram.Zoom;
            var deltaY = Diagram.Options.Zoom.Inverse ? e.DeltaY * -1 : e.DeltaY;
            var newZoom = deltaY > 0 ? oldZoom * scale : oldZoom / scale;

            if (newZoom < 0)
                return;

            // Other algorithms (based only on the changes in the zoom) don't work for our case
            // This solution is taken as is from react-diagrams (ZoomCanvasAction)
            var clientWidth = Diagram.Container.Width;
            var clientHeight = Diagram.Container.Height;
            var widthDiff = clientWidth * newZoom - clientWidth * oldZoom;
            var heightDiff = clientHeight * newZoom - clientHeight * oldZoom;
            var clientX = e.ClientX - Diagram.Container.Left;
            var clientY = e.ClientY - Diagram.Container.Top;
            var xFactor = (clientX - Diagram.Pan.X) / oldZoom / clientWidth;
            var yFactor = (clientY - Diagram.Pan.Y) / oldZoom / clientHeight;
            var newPanX = Diagram.Pan.X - widthDiff * xFactor;
            var newPanY = Diagram.Pan.Y - heightDiff * yFactor;

            newZoom = Math.Clamp(newZoom, Diagram.Options.Zoom.Minimum, Diagram.Options.Zoom.Maximum);
            if (newZoom == Diagram.Zoom)
                return;

            Diagram.Pan = new Point(newPanX, newPanY);
            Diagram.SetZoom(newZoom);
        }

        public override void Dispose()
        {
            Diagram.Wheel -= Diagram_Wheel;
        }
    }
}
