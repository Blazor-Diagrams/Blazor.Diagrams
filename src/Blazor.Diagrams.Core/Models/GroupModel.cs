using Blazor.Diagrams.Core.Models.Base;
using Blazor.Diagrams.Core.Models.Core;
using System.Linq;

namespace Blazor.Diagrams.Core.Models
{
    public class GroupModel : MovableModel
    {
        private readonly DiagramManager _diagramManager;

        public GroupModel(DiagramManager diagramManager, NodeModel[] nodes)
        {
            _diagramManager = diagramManager;
            Nodes = nodes;
            Initialize();
        }

        public NodeModel[] Nodes { get; private set; }
        public Size Size { get; private set; } = Size.Zero;

        public void Clear()
        {
            foreach (var node in Nodes)
            {
                node.Group = null;
                node.Moving -= Node_Moving;
            }
        }

        private void Initialize()
        {
            foreach (var node in Nodes)
            {
                node.Group = this;
                node.Moving += Node_Moving;
            }

            UpdateDimensions();
        }

        private void Node_Moving(NodeModel node) => UpdateDimensions();

        private void UpdateDimensions()
        {
            (var nodesMinX, var nodesMaxX, var nodesMinY, var nodesMaxY) = _diagramManager.GetNodesRect(Nodes.ToList());
            Size = new Size(nodesMaxX - nodesMinX, nodesMaxY - nodesMinY);
            Position = new Point(nodesMinX, nodesMinY);
            Refresh();
        }

        public override void SetPosition(double x, double y)
        {
            var deltaX = x - Position.X;
            var deltaY = y - Position.Y;
            base.SetPosition(x, y);

            foreach (var node in Nodes)
                node.UpdatePositionSilently(deltaX, deltaY);

            Refresh();
        }
    }
}
