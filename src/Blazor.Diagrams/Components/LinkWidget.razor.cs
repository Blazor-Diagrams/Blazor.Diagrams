using Blazor.Diagrams.Components.Base;
using System;
using System.Globalization;

namespace Blazor.Diagrams.Components
{
    public class LinkWidgetComponent : LinkWidgetBaseComponent
    {
        protected string GenerateCurvedPath()
        {
            var sX = Link.SourcePort.Position.X + (Link.SourcePort.Size.Width / 2);
            var sY = Link.SourcePort.Position.Y + (Link.SourcePort.Size.Height / 2);
            double tX, tY;

            if (Link.IsAttached)
            {
                tX = Link.TargetPort.Position.X + (Link.TargetPort.Size.Width / 2);
                tY = Link.TargetPort.Position.Y + (Link.TargetPort.Size.Height / 2);
            }
            else
            {
                tX = Link.OnGoingPosition.X;
                tY = Link.OnGoingPosition.Y;
            }

            var dist = Math.Abs(sX - tX);
            return $"M {sX} {sY} C {PrepNumber(sX + (dist / 2))} {sY}, {PrepNumber(tX - (dist / 2))} {tY}, {tX} {tY}";
        }

        private static string PrepNumber(double n) => n.ToString(CultureInfo.InvariantCulture);
    }
}
