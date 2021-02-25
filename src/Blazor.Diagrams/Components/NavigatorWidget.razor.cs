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

        [CascadingParameter(Name = "DiagramManager")]
        public DiagramManager DiagramManager { get; set; }

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

            foreach (var node in DiagramManager.Nodes)
                node.Changed += Refresh;

            foreach (var group in DiagramManager.Groups)
                group.Changed += Refresh;

            DiagramManager.Changed += DiagramManager_Changed;
            DiagramManager.Nodes.Added += DiagramManager_NodesAdded;
            DiagramManager.Nodes.Removed += DiagramManager_NodesRemoved;
            DiagramManager.GroupAdded += DiagramManager_GroupAdded;
            DiagramManager.GroupRemoved += DiagramManager_GroupRemoved;
        }

        private void DiagramManager_Changed() => Refresh();

        private void DiagramManager_NodesAdded(NodeModel[] nodes)
        {
            foreach (var node in nodes)
            {
                node.Changed += Refresh;
            }
        }

        private void DiagramManager_NodesRemoved(NodeModel[] nodes)
        {
            foreach (var node in nodes)
            {
                node.Changed -= Refresh;
            }
        }

        private void DiagramManager_GroupAdded(GroupModel group) => group.Changed += Refresh;

        private void DiagramManager_GroupRemoved(GroupModel group) => group.Changed -= Refresh;

        private void Refresh()
        {
            var nodes = DiagramManager.Nodes
                .Union(DiagramManager.Groups)
                .Where(n => n.Size?.Equals(Size.Zero) == false).ToList();

            if (nodes.Count == 0)
                return;

            (var nodesMinX, var nodesMaxX, var nodesMinY, var nodesMaxY) = nodes.GetBounds();
            nodesMinX *= DiagramManager.Zoom;
            nodesMaxX *= DiagramManager.Zoom;
            nodesMinY *= DiagramManager.Zoom;
            nodesMaxY *= DiagramManager.Zoom;

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
                var temp = nodesMinX + DiagramManager.Pan.X;
                if (DiagramManager.Pan.X > 0 && temp < 0)
                {
                    fullSizeWidth += Math.Abs(temp);
                }
                else if (DiagramManager.Pan.X <= 0)
                {
                    fullSizeWidth += Math.Abs(nodesMinX);
                }
            }

            // Height
            if (nodesMinY < 0)
            {
                var temp = nodesMinY + DiagramManager.Pan.Y;
                if (DiagramManager.Pan.Y > 0 && temp < 0)
                {
                    fullSizeHeight += Math.Abs(temp);
                }
                else if (DiagramManager.Pan.Y <= 0)
                {
                    fullSizeHeight += Math.Abs(nodesMinY);
                }
            }
        }

        private (double width, double height) GetFullSize(double nodesMaxX, double nodesMaxY)
        {
            var nodesLayerWidth = Math.Max(DiagramManager.Container.Width * DiagramManager.Zoom, nodesMaxX);
            var nodesLayerHeight = Math.Max(DiagramManager.Container.Height * DiagramManager.Zoom, nodesMaxY);
            double fullWidth;
            double fullHeight;

            if (DiagramManager.Zoom == 1)
            {
                fullWidth = DiagramManager.Container.Width + Math.Abs(DiagramManager.Pan.X);
                fullHeight = DiagramManager.Container.Height + Math.Abs(DiagramManager.Pan.Y);
            }
            else if (DiagramManager.Zoom > 1)
            {
                // Width
                if (DiagramManager.Pan.X < 0)
                {
                    if (nodesLayerWidth + DiagramManager.Pan.X < DiagramManager.Container.Width)
                    {
                        fullWidth = DiagramManager.Container.Width + Math.Abs(DiagramManager.Pan.X);
                    }
                    else
                    {
                        fullWidth = nodesLayerWidth;
                    }
                }
                else
                {
                    fullWidth = nodesLayerWidth + DiagramManager.Pan.X;
                }

                // Height
                if (DiagramManager.Pan.Y < 0)
                {
                    if (nodesLayerHeight + DiagramManager.Pan.Y < DiagramManager.Container.Height)
                    {
                        fullHeight = DiagramManager.Container.Height + Math.Abs(DiagramManager.Pan.Y);
                    }
                    else
                    {
                        fullHeight = nodesLayerHeight;
                    }
                }
                else
                {
                    fullHeight = nodesLayerHeight + DiagramManager.Pan.Y;
                }
            }
            else
            {
                // Width
                if (DiagramManager.Pan.X > 0)
                {
                    fullWidth = Math.Max(nodesLayerWidth + DiagramManager.Pan.X, DiagramManager.Container.Width);
                }
                else
                {
                    fullWidth = DiagramManager.Container.Width + Math.Abs(DiagramManager.Pan.X);
                }

                // Height
                if (DiagramManager.Pan.Y > 0)
                {
                    fullHeight = Math.Max(nodesLayerHeight + DiagramManager.Pan.Y, DiagramManager.Container.Height);
                }
                else
                {
                    fullHeight = DiagramManager.Container.Height + Math.Abs(DiagramManager.Pan.Y);
                }
            }

            return (fullWidth, fullHeight);
        }

        public void Dispose()
        {
            DiagramManager.Changed -= DiagramManager_Changed;
            DiagramManager.Nodes.Added -= DiagramManager_NodesAdded;
            DiagramManager.Nodes.Removed -= DiagramManager_NodesRemoved;

            // Todo: unregister node/group changed events
        }
    }
}