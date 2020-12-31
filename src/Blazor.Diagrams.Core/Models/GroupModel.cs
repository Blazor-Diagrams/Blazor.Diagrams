using Blazor.Diagrams.Core.Models.Core;
using System.Collections.Generic;
using System.Linq;

namespace Blazor.Diagrams.Core.Models
{
    public class GroupModel : NodeModel
    {
        private readonly DiagramManager _diagramManager;
        private readonly byte _padding;
        private readonly HashSet<LinkModel> _allLinks;

        public GroupModel(DiagramManager diagramManager, NodeModel[] children, byte padding = 30)
        {
            _diagramManager = diagramManager;
            _allLinks = new HashSet<LinkModel>();
            _padding = padding;

            Size = Size.Zero;
            Children = children;
            Initialize();
        }

        public NodeModel[] Children { get; private set; }
        public new IReadOnlyCollection<LinkModel> AllLinks => _allLinks;

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
            foreach (var node in Children)
            {
                node.Group = null;
                node.SizeChanged -= OnNodeChanged;
                node.Moving -= OnNodeChanged;
            }
        }

        private void Initialize()
        {
            foreach (var node in Children)
            {
                node.Group = this;
                node.SizeChanged += OnNodeChanged;
                node.Moving += OnNodeChanged;

                foreach (var link in node.AllLinks)
                {
                    if (link.SourcePort.Parent != node)
                        continue;

                    _allLinks.Add(link);
                }
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
