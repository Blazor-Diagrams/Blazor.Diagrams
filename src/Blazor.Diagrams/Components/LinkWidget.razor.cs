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

        protected string GenerateArrowToTargetPath()
        {
            return "";
        }

        protected string CalculateAngleForTargetArrow()
        {
            var p1 = Link.SourcePort.Position;
            var p2 = Link.IsAttached ? Link.TargetPort.Position : Link.OnGoingPosition;
            var angle = 90 + Math.Atan2(p2.Y - p1.Y, p2.X - p1.X) * 180 / Math.PI;
            return angle.ToString(CultureInfo.InvariantCulture);
        }

        private static string PrepNumber(double n) => n.ToString(CultureInfo.InvariantCulture);
    }
}
