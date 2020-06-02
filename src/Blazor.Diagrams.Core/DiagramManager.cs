using Blazor.Diagrams.Core.Default;
using Blazor.Diagrams.Core.Models;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Blazor.Diagrams")]
namespace Blazor.Diagrams.Core
{
    public class DiagramManager
    {
        private readonly List<Node> _nodes;
        private readonly List<DiagramSubManager> _subManagers;

        public event Action<Model, MouseEventArgs> MouseDown;
        public event Action<Model, MouseEventArgs> MouseMove;
        public event Action<Model, MouseEventArgs> MouseUp;

        public event Action<Node> NodeAdded;
        public event Action<Node> NodeRemoved;
        public event Action<Node, bool> NodeSelectionChanged;
        public event Action<Link> LinkAdded;

        public DiagramManager()
        {
            _nodes = new List<Node>();
            _subManagers = new List<DiagramSubManager>();

            RegisterSubManager<DragNodeSubManager>();
            RegisterSubManager<SelectionSubManager>();
        }

        public ReadOnlyCollection<Node> Nodes => _nodes.AsReadOnly();
        public IEnumerable<Link> AllLinks => _nodes.SelectMany(n => n.Ports.SelectMany(p => p.Links)).Distinct();
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

        public Link AddLink(Port source, Port target)
        {
            var link = new Link(source, target);
            source.AddLink(link);
            target.AddLink(link);
            LinkAdded?.Invoke(link);
            return link;
        }

        public void SelectNode(Node node)
        {
            if (SelectedNode == node)
                return;

            UnselectNode();
            SelectedNode = node;
            SelectedNode.Selected = true;
            NodeSelectionChanged?.Invoke(SelectedNode, true);
        }

        public void UnselectNode()
        {
            if (SelectedNode == null)
                return;

            var node = SelectedNode;
            node.Selected = false;
            node.Refresh();
            SelectedNode = null;
            NodeSelectionChanged?.Invoke(node, false);
        }

        public void RegisterSubManager<T>() where T : DiagramSubManager
        {
            var type = typeof(T);
            if (_subManagers.Any(sm => sm.GetType() == type))
                throw new Exception($"SubManager '{type.Name}' already registered.");

            var instance = (DiagramSubManager)Activator.CreateInstance(type, this);
            _subManagers.Add(instance);
        }

        public void UnregisterSubManager<T>() where T : DiagramSubManager
        {
            var subManager = _subManagers.FirstOrDefault(sm => sm.GetType() == typeof(T));
            if (subManager == null)
                return;

            subManager.Dispose();
            _subManagers.Remove(subManager);
        }

        internal void OnMouseDown(Model model, MouseEventArgs e) => MouseDown?.Invoke(model, e);

        internal void OnMouseMove(Model model, MouseEventArgs e) => MouseMove?.Invoke(model, e);

        internal void OnMouseUp(Model model, MouseEventArgs e) => MouseUp?.Invoke(model, e);
    }
}
