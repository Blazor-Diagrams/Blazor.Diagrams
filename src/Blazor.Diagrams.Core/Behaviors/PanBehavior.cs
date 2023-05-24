using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models.Base;
using Blazor.Diagrams.Core.Events;
using Blazor.Diagrams.Core.Behaviors.Base;

namespace Blazor.Diagrams.Core.Behaviors;

public class PanBehavior : DragBehavior
{
    private Point? _initialPan;
    private double _lastClientX;
    private double _lastClientY;

    public PanBehavior(Diagram diagram) : base(diagram)
    {
    }

    protected override void OnPointerDown(Model? model, PointerEventArgs e)
    {
        if (e.Button != (int)MouseEventButton.Left || model != null || !Diagram.Options.AllowPanning || !IsBehaviorEnabled(e))
            return;

        _initialPan = Diagram.Pan;
        _lastClientX = e.ClientX;
        _lastClientY = e.ClientY;
    }

    protected override void OnPointerMove(Model? model, PointerEventArgs e)
    {
        if (_initialPan == null)
            return;

        var deltaX = e.ClientX - _lastClientX - (Diagram.Pan.X - _initialPan.X);
        var deltaY = e.ClientY - _lastClientY - (Diagram.Pan.Y - _initialPan.Y);
        Diagram.UpdatePan(deltaX, deltaY);
    }

    protected override void OnPointerUp(Model? model, PointerEventArgs e)
    {
        _initialPan = null;
    }
}
