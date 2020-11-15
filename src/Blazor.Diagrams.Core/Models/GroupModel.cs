using Blazor.Diagrams.Core.Models.Base;
using Blazor.Diagrams.Core.Models.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Blazor.Diagrams.Core.Models
{
    public class GroupModel : Model
    {
        private readonly HashSet<NodeModel> _nodes = new HashSet<NodeModel>();
        private readonly DiagramManager _diagramManager;

        public GroupModel(DiagramManager diagramManager, RenderLayer layer)
        {
            _diagramManager = diagramManager;
            Layer = layer;
        }

        public RenderLayer Layer { get; }
        public IReadOnlyCollection<NodeModel> Nodes => _nodes;
        public Size Size { get; private set; } = Size.Zero;
        public Point Position { get; private set; } = Point.Zero;


        internal void AddNode(NodeModel node)
        {
            if (node.Layer != Layer)
                throw new ArgumentException($"The node must have be in the layer {Layer}");

            _nodes.Add(node);
            node.Changed += Node_Changed; // Todo: unsubscribe later
            Update();
        }

        internal void Clear() => _nodes.Clear();

        private void Node_Changed()
        {
            // Todo: optimize
            Update();
            Refresh();
        }

        private void Update()
        {
            if (_nodes.Count == 0)
                return;

            if (_nodes.Count == 1)
            {
                Size = _nodes.First().Size!;
                return;
            }

            (var nodesMinX, var nodesMaxX, var nodesMinY, var nodesMaxY) = _diagramManager.GetNodesRect(_nodes.ToList());
            Size = new Size(nodesMaxX - nodesMinX, nodesMaxY - nodesMinY);
            Position = new Point(nodesMinX, nodesMinY);
        }
    }
}
