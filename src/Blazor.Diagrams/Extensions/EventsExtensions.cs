using Blazor.Diagrams.Core.Events;
using System.Linq;
using Web = Microsoft.AspNetCore.Components.Web;

namespace Blazor.Diagrams.Extensions
{
    public static class EventsExtensions
    {
        public static PointerEventArgs ToCore(this Web.PointerEventArgs e)
        {
            return new PointerEventArgs(e.ClientX, e.ClientY, e.Button, e.Buttons, e.CtrlKey, e.ShiftKey, e.AltKey,
                e.PointerId, e.Width, e.Height, e.Pressure, e.TiltX, e.TiltY, e.PointerType, e.IsPrimary);
        }
        
        public static PointerEventArgs ToCore(this Web.MouseEventArgs e)
        {
            return new PointerEventArgs(e.ClientX, e.ClientY, e.Button, e.Buttons, e.CtrlKey, e.ShiftKey, e.AltKey,
                0, 0, 0, 0, 0, 0, string.Empty, false);
        }

        public static KeyboardEventArgs ToCore(this Web.KeyboardEventArgs e)
        {
            return new KeyboardEventArgs(e.Key, e.Code, e.Location, e.CtrlKey, e.ShiftKey, e.AltKey);
        }

        public static WheelEventArgs ToCore(this Web.WheelEventArgs e)
        {
            return new WheelEventArgs(e.ClientX, e.ClientY, e.Button, e.Buttons, e.CtrlKey, e.ShiftKey, e.AltKey, e.DeltaX, e.DeltaY, e.DeltaZ, e.DeltaMode);
        }
    }
}
