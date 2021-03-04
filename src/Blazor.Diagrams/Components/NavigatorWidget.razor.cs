using Blazor.Diagrams.Core;
using Blazor.Diagrams.Core.Extensions;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Core;
using Microsoft.AspNetCore.Components;
using System;
using System.Linq;

namespace Blazor.Diagrams.Components
{
    public partial class NavigatorWidget : IDisposable
    {

        [CascadingParameter]
        public Diagram Diagram { get; set; }

        [Parameter]
        public double Width { get; set; }

        [Parameter]
        public double Height { get; set; }

        [Parameter]
        public bool DefaultStyle { get; set; }

        protected double XFactor { get; set; }
        protected double YFactor { get; set; }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            foreach (var node in Diagram.Nodes)
                node.Changed += Refresh;

            foreach (var group in Diagram.Groups)
                group.Changed += Refresh;

            Diagram.Changed += Diagram_Changed;
            Diagram.Nodes.Added += Diagram_NodesAdded;
            Diagram.Nodes.Removed += Diagram_NodesRemoved;
            Diagram.GroupAdded += Diagram_GroupAdded;
            Diagram.GroupRemoved += Diagram_GroupRemoved;
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
            var nodes = Diagram.Nodes
                .Union(Diagram.Groups)
                .Where(n => n.Size?.Equals(Size.Zero) == false).ToList();

            if (nodes.Count == 0)
                return;

            var bounds = nodes.GetBounds();
            var nodesMinX = bounds.Left * Diagram.Zoom;
            var nodesMaxX = bounds.Right * Diagram.Zoom;
            var nodesMinY = bounds.Top * Diagram.Zoom;
            var nodesMaxY = bounds.Bottom * Diagram.Zoom;

            (double fullSizeWidth, double fullSizeHeight) = GetFullSize(nodesMaxX, nodesMaxY);
            AdjustFullSizeWithNodesRect(nodesMinX, nodesMinY, ref fullSizeWidth, ref fullSizeHeight);

            XFactor = Width / fullSizeWidth;
            YFactor = Height / fullSizeHeight;
            StateHasChanged();
        }

        private void AdjustFullSizeWithNodesRect(double nodesMinX, double nodesMinY, ref double fullSizeWidth,
            ref double fullSizeHeight)
        {
            // Width
            if (nodesMinX < 0)
            {
                var temp = nodesMinX + Diagram.Pan.X;
                if (Diagram.Pan.X > 0 && temp < 0)
                {
                    fullSizeWidth += Math.Abs(temp);
                }
                else if (Diagram.Pan.X <= 0)
                {
                    fullSizeWidth += Math.Abs(nodesMinX);
                }
            }

            // Height
            if (nodesMinY < 0)
            {
                var temp = nodesMinY + Diagram.Pan.Y;
                if (Diagram.Pan.Y > 0 && temp < 0)
                {
                    fullSizeHeight += Math.Abs(temp);
                }
                else if (Diagram.Pan.Y <= 0)
                {
                    fullSizeHeight += Math.Abs(nodesMinY);
                }
            }
        }

        private (double width, double height) GetFullSize(double nodesMaxX, double nodesMaxY)
        {
            var nodesLayerWidth = Math.Max(Diagram.Container.Width * Diagram.Zoom, nodesMaxX);
            var nodesLayerHeight = Math.Max(Diagram.Container.Height * Diagram.Zoom, nodesMaxY);
            double fullWidth;
            double fullHeight;

            if (Diagram.Zoom == 1)
            {
                fullWidth = Diagram.Container.Width + Math.Abs(Diagram.Pan.X);
                fullHeight = Diagram.Container.Height + Math.Abs(Diagram.Pan.Y);
            }
            else if (Diagram.Zoom > 1)
            {
                // Width
                if (Diagram.Pan.X < 0)
                {
                    if (nodesLayerWidth + Diagram.Pan.X < Diagram.Container.Width)
                    {
                        fullWidth = Diagram.Container.Width + Math.Abs(Diagram.Pan.X);
                    }
                    else
                    {
                        fullWidth = nodesLayerWidth;
                    }
                }
                else
                {
                    fullWidth = nodesLayerWidth + Diagram.Pan.X;
                }

                // Height
                if (Diagram.Pan.Y < 0)
                {
                    if (nodesLayerHeight + Diagram.Pan.Y < Diagram.Container.Height)
                    {
                        fullHeight = Diagram.Container.Height + Math.Abs(Diagram.Pan.Y);
                    }
                    else
                    {
                        fullHeight = nodesLayerHeight;
                    }
                }
                else
                {
                    fullHeight = nodesLayerHeight + Diagram.Pan.Y;
                }
            }
            else
            {
                // Width
                if (Diagram.Pan.X > 0)
                {
                    fullWidth = Math.Max(nodesLayerWidth + Diagram.Pan.X, Diagram.Container.Width);
                }
                else
                {
                    fullWidth = Diagram.Container.Width + Math.Abs(Diagram.Pan.X);
                }

                // Height
                if (Diagram.Pan.Y > 0)
                {
                    fullHeight = Math.Max(nodesLayerHeight + Diagram.Pan.Y, Diagram.Container.Height);
                }
                else
                {
                    fullHeight = Diagram.Container.Height + Math.Abs(Diagram.Pan.Y);
                }
            }

            return (fullWidth, fullHeight);
        }

        public void Dispose()
        {
            Diagram.Changed -= Diagram_Changed;
            Diagram.Nodes.Added -= Diagram_NodesAdded;
            Diagram.Nodes.Removed -= Diagram_NodesRemoved;

            // Todo: unregister node/group changed events
        }
    }
}