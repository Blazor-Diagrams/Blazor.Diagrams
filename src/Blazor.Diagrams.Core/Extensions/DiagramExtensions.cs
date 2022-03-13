using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Blazor.Diagrams.Core.Extensions
{
    public static class DiagramExtensions
    {
        public static Rectangle GetBounds(this IEnumerable<NodeModel> nodes)
        {
            if (!nodes.Any())
                return Rectangle.Zero;

            var minX = double.MaxValue;
            var maxX = double.MinValue;
            var minY = double.MaxValue;
            var maxY = double.MinValue;

            foreach (var node in nodes)
            {
                if (node.Size == null) // Ignore nodes that didn't get a size yet
                    continue;

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

        /// <summary>
        /// Gets a rectangle that represents the model's position
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static Rectangle GetBounds(this Model model)
        {
            if (model is NodeModel nodeModel)
            {
                return new Rectangle(nodeModel.Position, nodeModel.Size);
            }
            if (model is BaseLinkModel linkModel)
            {
                if (linkModel?.SourcePort?.Position != null && linkModel?.TargetPort?.Position != null)
                {
                    var minX = double.MaxValue;
                    var maxX = double.MinValue;
                    var minY = double.MaxValue;
                    var maxY = double.MinValue;

                    var linkX = linkModel.SourcePort.Position.X < linkModel.TargetPort.Position.X ? linkModel.SourcePort.Position.X : linkModel.TargetPort.Position.X;
                    var linkY = linkModel.SourcePort.Position.Y > linkModel.TargetPort.Position.Y ? linkModel.SourcePort.Position.Y : linkModel.TargetPort.Position.Y;
                    var Position = new Point(linkX, linkY);
                    var linkWidth = Math.Abs(linkModel.SourcePort.Position.X - linkModel.TargetPort.Position.X);
                    var linkHeight = Math.Abs(linkModel.SourcePort.Position.Y - linkModel.TargetPort.Position.Y);
                    var Size = new Size(linkWidth, linkHeight);

                    var trX = Position.X + Size!.Width;
                    var bY = Position.Y + Size.Height;

                    if (Position.X < minX)
                    {
                        minX = Position.X;
                    }
                    if (trX > maxX)
                    {
                        maxX = trX;
                    }
                    if (Position.Y < minY)
                    {
                        minY = Position.Y;
                    }
                    if (bY > maxY)
                    {
                        maxY = bY;
                    }

                    return new Rectangle(minX, minY, maxX, maxY);
                }

            }
            if (model is PortModel portModel)
            {
                return new Rectangle(portModel.MiddlePosition, portModel.Size);

            }
            if (model is GroupModel group)
            {
                return new Rectangle(group.Position, group.Size);
            }
            else
            {
                return Rectangle.Zero;
            }
        }
    }
}
