using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models.Base;
using Blazor.Diagrams.Core.Events;

namespace Blazor.Diagrams.Core.Behaviors;

public class PanBehavior : Behavior
{
    private readonly Dictionary<long, Point> _activePointers = new();
    private CenterCircle? _lastPointerCircle;

    public PanBehavior(Diagram diagram) : base(diagram)
    {
        Diagram.PointerDown += OnPointerDown;
        Diagram.PointerMove += OnPointerMove;
        Diagram.PointerUp += OnPointerUp;
    }

    private void OnPointerDown(Model? model, PointerEventArgs e)
    {
        if (e.Button != (int)MouseEventButton.Left)
            return;

        Start(model, e.Client, e.ShiftKey, e.PointerId);
    }

    private void OnPointerMove(Model? model, PointerEventArgs e) => Move(e.Client, e.PointerId);

    private void OnPointerUp(Model? model, PointerEventArgs e) => End(e.PointerId);

    private void Start(Model? model, Point client, bool shiftKey, long pointerId)
    {
        if (model != null || shiftKey)
            return;

        _activePointers[pointerId] = client;

        PointersChanged();
    }

    private void Move(Point client, long pointerId)
    {
        if (_lastPointerCircle == null)
            return;

        if (_activePointers.ContainsKey(pointerId) is false)
            return;

        _activePointers[pointerId] = client;

        var newPointerCircle = GetCurrentPointerCircle();

        var zoomFactor = _lastPointerCircle.Value.Radius == 0 ? 1 : newPointerCircle.Radius / _lastPointerCircle.Value.Radius;
        var deltaPan = newPointerCircle.Origin - _lastPointerCircle.Value.Origin;

        Diagram.Batch(() =>
        {
            if (Diagram.Options.Zoom.Enabled)
                Diagram.SetZoom(Diagram.Zoom * zoomFactor, zoomClientOrigin: newPointerCircle.Origin);
            if (Diagram.Options.AllowPanning)
                Diagram.UpdatePan(deltaPan.X, deltaPan.Y);
        });

        _lastPointerCircle = newPointerCircle;
    }

    private void End(long pointerId)
    {
        _activePointers.Remove(pointerId);

        if (_activePointers.Count is 0)
            _lastPointerCircle = null;
        else
            PointersChanged();
    }

    private CenterCircle GetCurrentPointerCircle()
    {
        var centroid = Point.CalculateCentroid(_activePointers.Values) ?? Point.Zero;
        var radius = _activePointers.Values.Average(p => p.DistanceTo(centroid));
        return new(centroid, radius);
    }

    private void PointersChanged() =>
        _lastPointerCircle = GetCurrentPointerCircle();

    public override void Dispose()
    {
        Diagram.PointerDown -= OnPointerDown;
        Diagram.PointerMove -= OnPointerMove;
        Diagram.PointerUp -= OnPointerUp;
    }

    private record struct CenterCircle(Point Origin, double Radius);
}
