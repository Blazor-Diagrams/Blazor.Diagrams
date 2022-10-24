using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;
using System;
using System.Globalization;

namespace Blazor.Diagrams.Core.Extensions
{
    [Obsolete]
    public static class BaseLinkModelExtensions
    {
        private const double _margin = 125;

        /// <summary>
        /// If the link is attached, returns the same output as GetMiddleTargetX().
        /// Otherwise, returns the X value of link's ongoing position.
        /// </summary>
        /// <param name="link">The BaseLinkModel entity</param>
        public static double GetTargetX(this BaseLinkModel link)
        {
            if (!link.IsAttached)
                return link.OnGoingPosition!.X;

            return link.GetMiddleTargetX();
        }

        /// <summary>
        /// If the link is attached, returns the same output as GetMiddleTargetY().
        /// Otherwise, returns the Y value of link's ongoing position.
        /// </summary>
        /// <param name="link">The BaseLinkModel entity</param>
        public static double GetTargetY(this BaseLinkModel link)
        {
            if (!link.IsAttached)
                return link.OnGoingPosition!.Y;

            return link.GetMiddleTargetY();
        }       

        public static string GenerateCurvedPath(this BaseLinkModel link)
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

            var curvePointA = GetCurvePoint(sX, sY, cX, cY, link.SourcePort?.Alignment);
            var curvePointB = GetCurvePoint(tX, tY, cX, cY, link.TargetPort?.Alignment);
            return FormattableString.Invariant($"M {sX} {sY} C {curvePointA}, {curvePointB}, {tX} {tY}");
        }

        private static string GetCurvePoint(double pX, double pY, double cX, double cY, PortAlignment? alignment)
        {
            var margin = Math.Min(_margin, Math.Pow(Math.Pow(pX - cX, 2) + Math.Pow(pY - cY, 2), .5));
            return alignment switch
            {
                PortAlignment.Top => FormattableString.Invariant($"{pX} {Math.Min(pY - margin, cY)}"),
                PortAlignment.Bottom => FormattableString.Invariant($"{pX} {Math.Max(pY + margin, cY)}"),
                PortAlignment.TopRight => FormattableString.Invariant($"{Math.Max(pX + margin, cX)} {Math.Min(pY - margin, cY)}"),
                PortAlignment.BottomRight => FormattableString.Invariant($"{Math.Max(pX + margin, cX)} {Math.Max(pY + margin, cY)}"),
                PortAlignment.Right => FormattableString.Invariant($"{Math.Max(pX + margin, cX)} {pY}"),
                PortAlignment.Left => FormattableString.Invariant($"{Math.Min(pX - margin, cX)} {pY}"),
                PortAlignment.BottomLeft => FormattableString.Invariant($"{Math.Min(pX - margin, cX)} {Math.Max(pY + margin, cY)}"),
                PortAlignment.TopLeft => FormattableString.Invariant($"{Math.Min(pX - margin, cX)} {Math.Min(pY - margin, cY)}"),
                _ => FormattableString.Invariant($"{cX} {cY}"),
            };
        }

        public static string CalculateAngleForTargetArrow(this BaseLinkModel link)
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

        public static double GetMiddleSourceX(this BaseLinkModel link)
            => link.SourcePort!.Position.X + (link.SourcePort.Size.Width / 2);

        public static double GetMiddleSourceY(this BaseLinkModel link)
            => link.SourcePort!.Position.Y + (link.SourcePort.Size.Height / 2);

        public static double GetMiddleTargetX(this BaseLinkModel link)
            => link.TargetPort!.Position.X + (link.TargetPort.Size.Width / 2);

        public static double GetMiddleTargetY(this BaseLinkModel link)
            => link.TargetPort!.Position.Y + (link.TargetPort.Size.Height / 2);
    }
}
