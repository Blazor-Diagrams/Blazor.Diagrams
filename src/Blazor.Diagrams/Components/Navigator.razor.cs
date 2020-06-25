using Blazor.Diagrams.Core;
using Blazor.Diagrams.Core.Default;
using Blazor.Diagrams.Core.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;

namespace Blazor.Diagrams.Components
{
    public class NavigatorComponent : ComponentBase, IDisposable
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
                node.Changed += Node_Changed;

            DiagramManager.Changed += DiagramManager_Changed;
            DiagramManager.NodeAdded += DiagramManager_NodeAdded;
            DiagramManager.NodeRemoved += DiagramManager_NodeRemoved;
        }

        private void DiagramManager_Changed() => Refresh();

        private void DiagramManager_NodeAdded(NodeModel node)
        {
            node.Changed += Node_Changed;
        }

        private void Node_Changed() => Refresh();

        private void DiagramManager_NodeRemoved(NodeModel node)
        {
            node.Changed -= Node_Changed;
        }

        private void Refresh()
        {
            var nodes = DiagramManager.Nodes.Where(n => n.Size != null).ToList();
            if (nodes.Count == 0)
                return;

            (var nodesMinX, var nodesMaxX, var nodesMinY, var nodesMaxY) = DiagramManager.GetNodesRect(nodes);
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
            DiagramManager.NodeAdded -= DiagramManager_NodeAdded;
            DiagramManager.NodeRemoved -= DiagramManager_NodeRemoved;
        }
    }
}