using Blazor.Diagrams.Core;
using Blazor.Diagrams.Core.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Linq;

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

        protected Rectangle AllNodesContainer { get; private set; }

        protected double XFactor { get; private set; }

        protected double YFactor { get; private set; }

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
            AllNodesContainer = GetAllNodesContainer();
            if (AllNodesContainer == null)
                return;

            XFactor = Width / AllNodesContainer.Width;
            YFactor = Height / AllNodesContainer.Height;
            StateHasChanged();
        }

        private Rectangle GetAllNodesContainer()
        {
            var nodes = DiagramManager.Nodes.Where(n => n.Size != null).ToArray();
            if (nodes.Length == 0)
                return null;

            var minX = nodes[0].Position.X;
            var maxX = nodes[0].Position.X + nodes[0].Size!.Width;
            var minY = nodes[0].Position.Y;
            var maxY = nodes[0].Position.Y + nodes[0].Size!.Height;

            for (var i = 1; i < nodes.Length; i++)
            {
                var node = nodes[i];
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

            return new Rectangle
            {
                Left = minX - 50 - DiagramManager.Pan.X,
                Top = minY - 50 - DiagramManager.Pan.Y,
                Width = DiagramManager.Pan.X + maxX - minX + 2 * 50,
                Height = DiagramManager.Pan.Y + maxY - minY + 2 * 50
            };
        }

        public void Dispose()
        {
            DiagramManager.Changed -= DiagramManager_Changed;
            DiagramManager.NodeAdded -= DiagramManager_NodeAdded;
            DiagramManager.NodeRemoved -= DiagramManager_NodeRemoved;
        }
    }
}