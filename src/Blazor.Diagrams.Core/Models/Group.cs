using Blazor.Diagrams.Core.Models.Base;
using System;
using System.Collections.Generic;

namespace Blazor.Diagrams.Core.Models
{
    public class Group : Model
    {
        private readonly HashSet<NodeModel> _nodes = new HashSet<NodeModel>();

        public Group(RenderLayer layer)
        {
            Layer = layer;
        }

        public RenderLayer Layer { get; }
        public IReadOnlyCollection<NodeModel> Nodes => _nodes;

        internal void AddNode(NodeModel node)
        {
            if (node.Layer != Layer)
                throw new ArgumentException($"The node must have be in the layer {Layer}");

            _nodes.Add(node);
        }

        internal void Clear() => _nodes.Clear();
    }
}
