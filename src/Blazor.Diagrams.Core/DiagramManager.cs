using Blazor.Diagrams.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Blazor.Diagrams.Core
{
    public class DiagramManager
    {
        private readonly List<Node> _nodes;

        public event Action<Node> NodeAdded;
        public event Action<Node> NodeRemoved;

        public DiagramManager()
        {
            _nodes = new List<Node>();
        }

        public ReadOnlyCollection<Node> Nodes => _nodes.AsReadOnly();

        public void AddNode(Node node)
        {
            _nodes.Add(node);
            NodeAdded?.Invoke(node);
        }

        public void RemoveNode(Node node)
        {
            if (_nodes.Remove(node))
            {
                NodeRemoved?.Invoke(node);
            }
        }
    }
}
