using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Core;
using System.Collections.Generic;

namespace Blazor.Diagrams.Core.Extensions
{
    public static class DiagramExtensions
    {
        public static Rectangle GetBounds(this IEnumerable<NodeModel> nodes)
        {
            var minX = double.MaxValue;
            var maxX = double.MinValue;
            var minY = double.MaxValue;
            var maxY = double.MinValue;

            foreach (var node in nodes)
            {
                var trX = node.Position.X + node.Size!.Width;
                var bY = node.Position.Y + node.Size.Height;

                if (node.Position.X < minX)
                {
                    minX = node.Position.X;
                }
                if (trX > maxX)
                {
                    maxX = trX;
                }
                if (node.Position.Y < minY)
                {
                    minY = node.Position.Y;
                }
                if (bY > maxY)
                {
                    maxY = bY;
                }
            }

            return new Rectangle(minX, minY, maxX, maxY);
        }
    }
}
