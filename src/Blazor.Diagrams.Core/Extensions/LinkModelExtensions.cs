using Blazor.Diagrams.Core.Models;
using System;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace Blazor.Diagrams.Core.Extensions
{
    public static class LinkModelExtensions
    {
        /// <summary>
        /// If the link is attached, returns the same output as GetMiddleTargetX().
        /// Otherwise, returns the X value of link's ongoing position.
        /// </summary>
        /// <param name="link">The LinkModel entity</param>
        public static double GetTargetX(this LinkModel link)
        {
            if (!link.IsAttached)
                return link.OnGoingPosition!.X;

            return link.GetMiddleTargetX();
        }

        /// <summary>
        /// If the link is attached, returns the same output as GetMiddleTargetY().
        /// Otherwise, returns the Y value of link's ongoing position.
        /// </summary>
        /// <param name="link">The LinkModel entity</param>
        public static double GetTargetY(this LinkModel link)
        {
            if (!link.IsAttached)
                return link.OnGoingPosition!.Y;

            return link.GetMiddleTargetY();
        }       

        public static string GenerateCurvedPath(this LinkModel link)
        {
            var sX = link.GetMiddleSourceX();
            var sY = link.GetMiddleSourceY();
            double tX, tY;

            if (link.IsAttached)
            {
                tX = link.GetMiddleTargetX();
                tY = link.GetMiddleTargetY();
            }
            else
            {
                tX = link.OnGoingPosition!.X;
                tY = link.OnGoingPosition.Y;
            }

            var cX = (sX + tX) / 2;
            var cY = (sY + tY) / 2;

            var curvePointA = GetCurvePoint(sX, sY, cX, cY, link.SourcePort.Alignment);
            var curvePointB = GetCurvePoint(tX, tY, cX, cY, link.TargetPort?.Alignment);

            return FormattableString.Invariant($"M {sX} {sY} C {curvePointA}, {curvePointB}, {tX} {tY}");
        }

        private const double Margin = 125;
        private static string GetCurvePoint(double pX, double pY, double cX, double cY, PortAlignment? alignment)
        {
            switch (alignment)
            {
                case PortAlignment.Top:
                    return FormattableString.Invariant($"{pX} {Math.Min(pY - Margin, cY)}");
                case PortAlignment.Bottom:
                    return FormattableString.Invariant($"{pX} {Math.Max(pY + Margin, cY)}");
                case PortAlignment.TopRight:
                    return FormattableString.Invariant($"{Math.Max(pX + Margin, cX)} {Math.Min(pY - Margin, cY)}");
                case PortAlignment.BottomRight:
                    return FormattableString.Invariant($"{Math.Max(pX + Margin, cX)} {Math.Max(pY + Margin, cY)}");                    
                case PortAlignment.Right:
                    return FormattableString.Invariant($"{Math.Max(pX + Margin, cX)} {pY}");
                case PortAlignment.Left:
                    return FormattableString.Invariant($"{Math.Min(pX - Margin, cX)} {pY}");
                case PortAlignment.BottomLeft:
                    return FormattableString.Invariant($"{Math.Min(pX - Margin, cX)} {Math.Max(pY + Margin, cY)}");
                case PortAlignment.TopLeft:
                    return FormattableString.Invariant($"{Math.Min(pX - Margin, cX)} {Math.Min(pY - Margin, cY)}");
            }
            return FormattableString.Invariant($"{cX} {cY}");
        }

        public static string CalculateAngleForTargetArrow(this LinkModel link)
        {
            var sX = link.GetMiddleSourceX();
            var sY = link.GetMiddleSourceY();
            double tX, tY;

            if (link.IsAttached)
            {
                tX = link.GetMiddleTargetX();
                tY = link.GetMiddleTargetY();
            }
            else
            {
                tX = link.OnGoingPosition!.X;
                tY = link.OnGoingPosition.Y;
            }

            var angle = 90 + Math.Atan2(tY - sY, tX - sX) * 180 / Math.PI;
            return angle.ToString(CultureInfo.InvariantCulture);
        }

        public static double GetMiddleSourceX(this LinkModel link)
            => link.SourcePort.Position.X + (link.SourcePort.Size.Width / 2);

        public static double GetMiddleSourceY(this LinkModel link)
            => link.SourcePort.Position.Y + (link.SourcePort.Size.Height / 2);

        public static double GetMiddleTargetX(this LinkModel link)
            => link.TargetPort!.Position.X + (link.TargetPort.Size.Width / 2);

        public static double GetMiddleTargetY(this LinkModel link)
            => link.TargetPort!.Position.Y + (link.TargetPort.Size.Height / 2);
    }
}
