using Blazor.Diagrams.Core.Geometry;

namespace Blazor.Diagrams.Core.Events;

public record MouseEventArgs(double ClientX, double ClientY, long Button, long Buttons, bool CtrlKey, bool ShiftKey, bool AltKey)
{
    public Point Client => new(ClientX, ClientY);
}
