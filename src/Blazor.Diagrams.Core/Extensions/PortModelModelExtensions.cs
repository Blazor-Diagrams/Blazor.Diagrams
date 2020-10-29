using System;
using Blazor.Diagrams.Core.Models;

namespace Blazor.Diagrams.Core.Extensions
{
    public static class PortModelModelExtensions
    {
        /// <summary>
        /// Gets the distance between two ports.
        /// </summary>
        /// <param name="thisPort"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public static double GetDistance(this PortModel thisPort, PortModel port)
        {
            //Good old Pythagoras
            var xDistance = Math.Abs(thisPort.Position.X - port.Position.X);
            var yDistance = Math.Abs(thisPort.Position.Y - port.Position.Y);
            return Math.Sqrt(xDistance * xDistance + yDistance * yDistance);
        }
    }
}