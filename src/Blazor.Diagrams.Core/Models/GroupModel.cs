using Blazor.Diagrams.Core.Models.Base;
using Blazor.Diagrams.Core.Models.Core;
using System.Linq;

namespace Blazor.Diagrams.Core.Models
{
    public class GroupModel : MovableModel
    {
        private readonly DiagramManager _diagramManager;

        public GroupModel(DiagramManager diagramManager, NodeModel[] children)
        {
            _diagramManager = diagramManager;
            Children = children;
            Initialize();
        }

        public NodeModel[] Children { get; private set; }
        public Size Size { get; private set; } = Size.Zero;

        public override void SetPosition(double x, double y)
        {
            var deltaX = x - Position.X;
            var deltaY = y - Position.Y;
            base.SetPosition(x, y);

            foreach (var node in Children)
                node.UpdatePositionSilently(deltaX, deltaY);

            Refresh();
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
            Size = new Size(nodesMaxX - nodesMinX, nodesMaxY - nodesMinY);
            Position = new Point(nodesMinX, nodesMinY);
        }
    }
}
