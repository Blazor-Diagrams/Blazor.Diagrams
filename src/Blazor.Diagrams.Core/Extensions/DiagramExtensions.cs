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

        public static Rectangle GetBounds(Point Position, Size Size)
        {
            var minX = double.MaxValue;
            var maxX = double.MinValue;
            var minY = double.MaxValue;
            var maxY = double.MinValue;


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

        /// <summary>
        /// Gets a rectangle that represents the model's position
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static Rectangle GetBounds(this Model model)
        {
            if (model is GroupModel group)
            {
                return new Rectangle(group.Position, group.Size);
            }
            if (model is NodeModel nodeModel)
            {
                return new Rectangle(nodeModel.Position, nodeModel.Size);
            }
            if (model is BaseLinkModel linkModel)
            {
                //For the link its usefull to see what its connected to
                if (linkModel?.SourceNode?.Position != null && linkModel?.TargetNode?.Position != null)
                {

                    var smallestXNode = linkModel.SourceNode.Position.X < linkModel.TargetNode.Position.X ? linkModel.SourceNode : linkModel.TargetNode;
                    var biggestXNode = linkModel.SourceNode.Position.X > linkModel.TargetNode.Position.X ? linkModel.SourceNode : linkModel.TargetNode;
                    var biggestYNode = linkModel.SourceNode.Position.Y > linkModel.TargetNode.Position.Y ? linkModel.SourceNode : linkModel.TargetNode;
                    var smallestYNode = linkModel.SourceNode.Position.Y < linkModel.TargetNode.Position.Y ? linkModel.SourceNode : linkModel.TargetNode;

                    var Position = new Point(smallestXNode.Position.X, smallestYNode.Position.Y);

                    var linkWidth = Math.Abs(biggestXNode.Position.X - smallestXNode.Position.X + biggestXNode.Size.Width);
                    var linkHeight = Math.Abs(biggestYNode.Position.Y - smallestYNode.Position.Y + biggestYNode.Size.Height);


                    var Size = new Size(linkWidth, linkHeight);
                    return GetBounds(Position, Size);
                }

            }
            if (model is PortModel portModel)
            {
                return new Rectangle(portModel.MiddlePosition, portModel.Size);
            }
            else
            {
                return Rectangle.Zero;
            }
        }
    }
}
