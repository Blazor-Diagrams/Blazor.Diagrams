using Blazor.Diagrams.Core;
using Blazor.Diagrams.Core.Extensions;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Linq;

namespace Blazor.Diagrams.Components
{
    public partial class NavigatorWidget : IDisposable
    {
        [CascadingParameter] public BlazorDiagram BlazorDiagram { get; set; }
        [Parameter] public double Width { get; set; }
        [Parameter] public double Height { get; set; }
        [Parameter] public string FillColor { get; set; } = "#40babd";
        [Parameter] public bool DefaultStyle { get; set; }

        private Point NodePositionAdjustment { get; set; }
        private double XFactor { get; set; }
        private double YFactor { get; set; }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            if (BlazorDiagram != null)
            {
                foreach (var node in BlazorDiagram.Nodes)
                    node.Changed += Refresh;

                foreach (var group in BlazorDiagram.Groups)
                    group.Changed += Refresh;

                BlazorDiagram.Changed += Diagram_Changed;
                BlazorDiagram.Nodes.Added += Diagram_NodesAdded;
                BlazorDiagram.Nodes.Removed += Diagram_NodesRemoved;
                BlazorDiagram.GroupAdded += Diagram_GroupAdded;
                BlazorDiagram.GroupRemoved += Diagram_GroupRemoved;
            }

        }

        private void Diagram_Changed() => Refresh();

        private void Diagram_NodesAdded(NodeModel node)
        {
            node.Changed += Refresh;
        }

        private void Diagram_NodesRemoved(NodeModel node)
        {
            node.Changed -= Refresh;
        }

        private void Diagram_GroupAdded(GroupModel group) => group.Changed += Refresh;

        private void Diagram_GroupRemoved(GroupModel group) => group.Changed -= Refresh;

        private void Refresh()
        {
            if (BlazorDiagram != null)
            {
                var nodes = BlazorDiagram.Nodes
              .Union(BlazorDiagram.Groups)
              .Where(n => n.Size?.Equals(Size.Zero) == false).ToList();

                if (nodes.Count == 0)
                    return;

                var bounds = nodes.GetBounds();
                var nodesMinX = bounds.Left * BlazorDiagram.Zoom;
                var nodesMaxX = bounds.Right * BlazorDiagram.Zoom;
                var nodesMinY = bounds.Top * BlazorDiagram.Zoom;
                var nodesMaxY = bounds.Bottom * BlazorDiagram.Zoom;

                (double fullSizeWidth, double fullSizeHeight) = GetFullSize(nodesMaxX, nodesMaxY);
                AdjustFullSizeWithNodesRect(nodesMinX, nodesMinY, ref fullSizeWidth, ref fullSizeHeight);

                NodePositionAdjustment = new Point(nodesMinX < 0 ? Math.Abs(nodesMinX) : 0, nodesMinY < 0 ? Math.Abs(nodesMinY) : 0);
                XFactor = Width / fullSizeWidth;
                YFactor = Height / fullSizeHeight;
                InvokeAsync(StateHasChanged);
            }

        }

        private void AdjustFullSizeWithNodesRect(double nodesMinX, double nodesMinY, ref double fullSizeWidth,
            ref double fullSizeHeight)
        {
            // Width
            if (nodesMinX < 0)
            {
                var temp = nodesMinX + BlazorDiagram.Pan.X;
                if (BlazorDiagram.Pan.X > 0 && temp < 0)
                {
                    fullSizeWidth += Math.Abs(temp);
                }
                else if (BlazorDiagram.Pan.X <= 0)
                {
                    fullSizeWidth += Math.Abs(nodesMinX);
                }
            }

            // Height
            if (nodesMinY < 0)
            {
                var temp = nodesMinY + BlazorDiagram.Pan.Y;
                if (BlazorDiagram.Pan.Y > 0 && temp < 0)
                {
                    fullSizeHeight += Math.Abs(temp);
                }
                else if (BlazorDiagram.Pan.Y <= 0)
                {
                    fullSizeHeight += Math.Abs(nodesMinY);
                }
            }
        }

        private (double width, double height) GetFullSize(double nodesMaxX, double nodesMaxY)
        {
            var nodesLayerWidth = Math.Max(BlazorDiagram.Container.Width * BlazorDiagram.Zoom, nodesMaxX);
            var nodesLayerHeight = Math.Max(BlazorDiagram.Container.Height * BlazorDiagram.Zoom, nodesMaxY);
            double fullWidth;
            double fullHeight;

            if (BlazorDiagram.Zoom == 1)
            {
                fullWidth = BlazorDiagram.Container.Width + Math.Abs(BlazorDiagram.Pan.X);
                fullHeight = BlazorDiagram.Container.Height + Math.Abs(BlazorDiagram.Pan.Y);
            }
            else if (BlazorDiagram.Zoom > 1)
            {
                // Width
                if (BlazorDiagram.Pan.X < 0)
                {
                    if (nodesLayerWidth + BlazorDiagram.Pan.X < BlazorDiagram.Container.Width)
                    {
                        fullWidth = BlazorDiagram.Container.Width + Math.Abs(BlazorDiagram.Pan.X);
                    }
                    else
                    {
                        fullWidth = nodesLayerWidth;
                    }
                }
                else
                {
                    fullWidth = nodesLayerWidth + BlazorDiagram.Pan.X;
                }

                // Height
                if (BlazorDiagram.Pan.Y < 0)
                {
                    if (nodesLayerHeight + BlazorDiagram.Pan.Y < BlazorDiagram.Container.Height)
                    {
                        fullHeight = BlazorDiagram.Container.Height + Math.Abs(BlazorDiagram.Pan.Y);
                    }
                    else
                    {
                        fullHeight = nodesLayerHeight;
                    }
                }
                else
                {
                    fullHeight = nodesLayerHeight + BlazorDiagram.Pan.Y;
                }
            }
            else
            {
                // Width
                if (BlazorDiagram.Pan.X > 0)
                {
                    fullWidth = Math.Max(nodesLayerWidth + BlazorDiagram.Pan.X, BlazorDiagram.Container.Width);
                }
                else
                {
                    fullWidth = BlazorDiagram.Container.Width + Math.Abs(BlazorDiagram.Pan.X);
                }

                // Height
                if (BlazorDiagram.Pan.Y > 0)
                {
                    fullHeight = Math.Max(nodesLayerHeight + BlazorDiagram.Pan.Y, BlazorDiagram.Container.Height);
                }
                else
                {
                    fullHeight = BlazorDiagram.Container.Height + Math.Abs(BlazorDiagram.Pan.Y);
                }
            }

            return (fullWidth, fullHeight);
        }

        public void Dispose()
        {
            if (BlazorDiagram != null)
            {
                BlazorDiagram.Changed -= Diagram_Changed;
                BlazorDiagram.Nodes.Added -= Diagram_NodesAdded;
                BlazorDiagram.Nodes.Removed -= Diagram_NodesRemoved;
                foreach (var node in BlazorDiagram.Nodes)
                    node.Changed -= Refresh;

                foreach (var group in BlazorDiagram.Groups)
                    group.Changed -= Refresh;
            }
        }
    }
}