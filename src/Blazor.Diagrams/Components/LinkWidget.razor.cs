using Blazor.Diagrams.Core.Extensions;
using Blazor.Diagrams.Core.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Globalization;

namespace Blazor.Diagrams.Components
{
    public partial class LinkWidget
    {
        [Parameter]
        public LinkModel Link { get; set; }

        protected string GetTargetX()
        {
            if (!Link.IsAttached)
                return Link.OnGoingPosition.X.ToInvariantString();

            return MiddleTargetX.ToInvariantString();
        }

        protected string GetTargetY()
        {
            if (!Link.IsAttached)
                return Link.OnGoingPosition.Y.ToInvariantString();

            return MiddleTargetY.ToInvariantString();
        }

        protected string GenerateCurvedPath()
        {
            var sX = MiddleSourceX;
            var sY = MiddleSourceY;
            double tX, tY;

            if (Link.IsAttached)
            {
                tX = MiddleTargetX;
                tY = MiddleTargetY;
            }
            else
            {
                tX = Link.OnGoingPosition.X;
                tY = Link.OnGoingPosition.Y;
            }

            return $"M {sX.ToInvariantString()} {sY.ToInvariantString()} " +
                $"C {((sX + tX) / 2).ToInvariantString()} {sY.ToInvariantString()}," +
                $" {((sX + tX) / 2).ToInvariantString()} {tY.ToInvariantString()}," +
                $" {tX.ToInvariantString()} {tY.ToInvariantString()}";
        }

        protected string CalculateAngleForTargetArrow()
        {
            var sX = MiddleSourceX;
            var sY = MiddleSourceY;
            double tX, tY;

            if (Link.IsAttached)
            {
                tX = MiddleTargetX;
                tY = MiddleTargetY;
            }
            else
            {
                tX = Link.OnGoingPosition.X;
                tY = Link.OnGoingPosition.Y;
            }

            var angle = 90 + Math.Atan2(tY - sY, tX - sX) * 180 / Math.PI;
            return angle.ToString(CultureInfo.InvariantCulture);
        }

        protected double MiddleSourceX
            => Link.SourcePort.Position.X + (Link.SourcePort.Size.Width / 2);

        protected double MiddleSourceY
            => Link.SourcePort.Position.Y + (Link.SourcePort.Size.Height / 2);

        protected double MiddleTargetX
            => Link.TargetPort.Position.X + (Link.SourcePort.Size.Width / 2);

        protected double MiddleTargetY
            => Link.TargetPort.Position.Y + (Link.SourcePort.Size.Height / 2);
    }
}
