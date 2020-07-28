using Blazor.Diagrams.Core.Models.Base;
using System.Collections.Generic;

namespace Blazor.Diagrams.Core.Models
{
    public class Group : Model
    {
        private readonly HashSet<NodeModel> _nodes = new HashSet<NodeModel>();

        public IReadOnlyCollection<NodeModel> Nodes => _nodes;

        internal void AddNode(NodeModel node) => _nodes.Add(node);
        internal void Clear() => _nodes.Clear();
    }
}
