using Blazor.Diagrams.Core.Models;
using System;
using System.Globalization;

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

            return $"M {sX.ToInvariantString()} {sY.ToInvariantString()} " +
                $"C {((sX + tX) / 2).ToInvariantString()} {sY.ToInvariantString()}," +
                $" {((sX + tX) / 2).ToInvariantString()} {tY.ToInvariantString()}," +
                $" {tX.ToInvariantString()} {tY.ToInvariantString()}";
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
            => link.TargetPort!.Position.X + (link.SourcePort.Size.Width / 2);

        public static double GetMiddleTargetY(this LinkModel link)
            => link.TargetPort!.Position.Y + (link.SourcePort.Size.Height / 2);



        /// <summary>
        /// Reconnects the link to the ports that are closest in distance for the link's nodes.
        /// </summary>
        /// <param name="link"></param>
        /// <returns>The reconnected LinkModel entitiy</returns>
        public static T Reconnect<T>(this T link) where T:LinkModel
        {
            if (link.TargetPort == null)
            {
                return link;
            }

            var sourcePorts = link.SourcePort.Parent.Ports;
            var targetPorts = link.TargetPort.Parent.Ports;

            //Find the ports with minimal distance
            var minDistance = double.MaxValue;
            var minSourcePort = link.SourcePort;
            var minTargetPort = link.TargetPort;
            foreach (var sourcePort in sourcePorts)
            {
                foreach (var targetPort in targetPorts)
                {
                    var distance = sourcePort.GetDistance(targetPort);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        minSourcePort = sourcePort;
                        minTargetPort = targetPort;
                    }
                }
            }

            //Reconnect
            link.SetSourcePort(minSourcePort);
            link.SetTargetPort(minTargetPort);

            return link;
        }
    }
}
