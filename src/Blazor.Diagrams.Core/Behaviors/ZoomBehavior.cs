using Blazor.Diagrams.Core.Events;

using System;

namespace Blazor.Diagrams.Core.Behaviors;

public class ZoomBehavior : Behavior
{
    public ZoomBehavior(Diagram diagram) : base(diagram)
    {
        Diagram.Wheel += Diagram_Wheel;
    }

    private void Diagram_Wheel(WheelEventArgs e)
    {
        if (Diagram.Container == null || e.DeltaY == 0)
            return;

        if (!Diagram.Options.Zoom.Enabled)
            return;

        var scale = Diagram.Options.Zoom.ScaleFactor;
        var oldZoom = Diagram.Zoom;
        var deltaY = Diagram.Options.Zoom.Inverse ? e.DeltaY * -1 : e.DeltaY;
        var newZoom = deltaY > 0 ? oldZoom * scale : oldZoom / scale;
        newZoom = Math.Clamp(newZoom, Diagram.Options.Zoom.Minimum, Diagram.Options.Zoom.Maximum);

        if (newZoom < 0 || newZoom == Diagram.Zoom)
            return;

        Diagram.SetZoom(newZoom, zoomClientOrigin: e.Client);
    }

    public override void Dispose()
    {
        Diagram.Wheel -= Diagram_Wheel;
    }
}
