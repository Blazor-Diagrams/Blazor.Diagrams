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
        public Node SelectedNode { get; private set; }

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

        public void OnMouseDown(Node node, double[] offsets, double clientX, double clientY)
        {
            node.Selected = true;
            node.Offset = new Point(offsets[0] - clientX, offsets[1] - clientY);
            SelectNode(node);
        }

        public void SelectNode(Node node)
        {
            if (SelectedNode == node)
                return;

            if (SelectedNode != null)
            {
                SelectedNode.Selected = false;
                SelectedNode.Refresh();
            }

            SelectedNode = node;
        }
    }
}
