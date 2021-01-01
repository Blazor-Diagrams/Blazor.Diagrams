using Blazor.Diagrams.Core.Models.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Blazor.Diagrams.Core.Models
{
    public class GroupModel : NodeModel
    {
        private readonly DiagramManager _diagramManager;
        private readonly byte _padding;

        public GroupModel(DiagramManager diagramManager, NodeModel[] children, byte padding = 30)
        {
            _diagramManager = diagramManager;
            _padding = padding;

            Size = Size.Zero;
            Children = children;
            Initialize();
        }

        public NodeModel[] Children { get; private set; }
        public IEnumerable<LinkModel> HandledLinks => Group != null ? Array.Empty<LinkModel>() : Children.SelectMany(c => c.AllLinks).Distinct();

        public override void SetPosition(double x, double y)
        {
            var deltaX = x - Position.X;
            var deltaY = y - Position.Y;
            base.SetPosition(x, y);

            foreach (var node in Children)
                node.UpdatePositionSilently(deltaX, deltaY);

            Refresh();
        }

        public override void UpdatePositionSilently(double deltaX, double deltaY)
        {
            base.UpdatePositionSilently(deltaX, deltaY);

            foreach (var child in Children)
                child.UpdatePositionSilently(deltaX, deltaY);
        }

        public void Ungroup()
        {
            foreach (var child in Children)
            {
                child.Group = null;
                child.SizeChanged -= OnNodeChanged;
                child.Moving -= OnNodeChanged;
            }
        }

        private void Initialize()
        {
            foreach (var child in Children)
            {
                child.Group = this;
                child.SizeChanged += OnNodeChanged;
                child.Moving += OnNodeChanged;
            }

            UpdateDimensions();
        }

        private void OnNodeChanged(NodeModel node)
        {
            UpdateDimensions();
            Refresh();
        }

        private void UpdateDimensions()
        {
            if (Children.Any(n => n.Size == null))
                return;

            (var nodesMinX, var nodesMaxX, var nodesMinY, var nodesMaxY) = _diagramManager.GetNodesRect(Children);
            Size = new Size(nodesMaxX - nodesMinX + _padding * 2, nodesMaxY - nodesMinY + _padding * 2);
            Position = new Point(nodesMinX - _padding, nodesMinY - _padding);
        }
    }
}
