using Blazor.Diagrams.Core.Events;
using System.Linq;
using Web = Microsoft.AspNetCore.Components.Web;

namespace Blazor.Diagrams.Extensions
{
    public static class EventsExtensions
    {
        public static MouseEventArgs ToCore(this Web.MouseEventArgs e)
        {
            return new MouseEventArgs(e.ClientX, e.ClientY, e.Button, e.Buttons, e.CtrlKey, e.ShiftKey, e.AltKey);
        }

        public static KeyboardEventArgs ToCore(this Web.KeyboardEventArgs e)
        {
            return new KeyboardEventArgs(e.Key, e.Code, e.Location, e.CtrlKey, e.ShiftKey, e.AltKey);
        }

        public static WheelEventArgs ToCore(this Web.WheelEventArgs e)
        {
            return new WheelEventArgs(e.ClientX, e.ClientY, e.Button, e.Buttons, e.CtrlKey, e.ShiftKey, e.AltKey, e.DeltaX, e.DeltaY, e.DeltaZ, e.DeltaMode);
        }

        public static TouchEventArgs ToCore(this Web.TouchEventArgs e)
        {
            return new TouchEventArgs(e.ChangedTouches.Select(ToCore).ToArray(), e.CtrlKey, e.ShiftKey, e.AltKey);
        }

        public static TouchPoint ToCore(this Web.TouchPoint e)
        {
            return new TouchPoint(e.Identifier, e.ClientX, e.ClientY);
        }
    }
}
