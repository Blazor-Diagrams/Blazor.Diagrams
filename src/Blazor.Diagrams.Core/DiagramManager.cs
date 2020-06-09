using Blazor.Diagrams.Core.Default;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Blazor.Diagrams")]
namespace Blazor.Diagrams.Core
{
    public class DiagramManager
    {
        private readonly List<NodeModel> _nodes;
        private readonly List<DiagramSubManager> _subManagers;
        private readonly Dictionary<Type, Type> _componentByModelMapping;
        private readonly HashSet<SelectableModel> _selectedModels;

        public event Action<Model, MouseEventArgs> MouseDown;
        public event Action<Model, MouseEventArgs> MouseMove;
        public event Action<Model, MouseEventArgs> MouseUp;
        public event Action<KeyboardEventArgs> KeyDown;
        public event Action<WheelEventArgs> Wheel;

        public event Action Changed;
        public event Action<NodeModel> NodeAdded;
        public event Action<NodeModel> NodeRemoved;
        public event Action<SelectableModel, bool> SelectionChanged;
        public event Action<LinkModel> LinkAdded;
        public event Action<LinkModel> LinkAttached;
        public event Action<LinkModel> LinkRemoved;

        public DiagramManager()
        {
            _nodes = new List<NodeModel>();
            _subManagers = new List<DiagramSubManager>();
            _componentByModelMapping = new Dictionary<Type, Type>();
            _selectedModels = new HashSet<SelectableModel>();

            RegisterSubManager<SelectionSubManager>();
            RegisterSubManager<DragNodeSubManager>();
            RegisterSubManager<DragNewLinkSubManager>();
            RegisterSubManager<DeleteSelectionSubManager>();
            RegisterSubManager<PanSubManager>();
            RegisterSubManager<ZoomSubManager>();
        }

        public IReadOnlyCollection<NodeModel> Nodes => _nodes;
        public IEnumerable<LinkModel> AllLinks => _nodes.SelectMany(n => n.AllLinks).Distinct();
        public IReadOnlyCollection<SelectableModel> SelectedModels => _selectedModels;
        public Rectangle Container { get; internal set; }
        public Point Pan { get; internal set; } = Point.Zero;
        public float Zoom { get; internal set; } = 1;

        public void AddNode(NodeModel node)
        {
            _nodes.Add(node);
            NodeAdded?.Invoke(node);
            Changed?.Invoke();
        }

        public void RemoveNode(NodeModel node, bool triggerEvent = true)
        {
            if (_nodes.Remove(node))
            {
                foreach (var link in node.AllLinks.ToList()) // Since we're removing from the list
                {
                    RemoveLink(link, false);
                }

                if (triggerEvent)
                {
                    NodeRemoved?.Invoke(node);
                    Changed?.Invoke();
                }
            }
        }

        public LinkModel AddLink(PortModel source, PortModel? target = null, Point? onGoingPosition = null)
        {
            var link = new LinkModel(source, target);
            source.AddLink(link);
            target?.AddLink(link);

            if (target == null)
            {
                link.OnGoingPosition = onGoingPosition ?? Point.Zero;
            }

            LinkAdded?.Invoke(link);
            Changed?.Invoke();
            return link;
        }

        public void AttachLink(LinkModel link, PortModel targetPort)
        {
            if (link.IsAttached)
                throw new Exception("Link already attached.");

            if (!link.SourcePort.CanAttachTo(targetPort))
                return;

            link.SetTargetPort(targetPort);
            targetPort.AddLink(link);
            link.Refresh();
            LinkAttached?.Invoke(link);
        }

        public void RemoveLink(LinkModel link, bool triggerEvent = true)
        {
            link.SourcePort.RemoveLink(link);
            link.TargetPort?.RemoveLink(link);

            if (triggerEvent)
            {
                LinkRemoved?.Invoke(link);
                Changed?.Invoke();
            }
        }

        public void SelectModel(SelectableModel model, bool unselectOthers)
        {
            if (_selectedModels.Contains(model))
                return;

            if (unselectOthers)
                UnselectAll();

            model.Selected = true;
            _selectedModels.Add(model);
            model.Refresh();
            SelectionChanged?.Invoke(model, true);
        }

        public void UnselectModel(SelectableModel model)
        {
            if (!_selectedModels.Contains(model))
                return;

            model.Selected = false;
            _selectedModels.Remove(model);
            model.Refresh();
            SelectionChanged?.Invoke(model, false);
        }

        public void UnselectAll()
        {
            foreach (var model in _selectedModels)
            {
                model.Selected = false;
                model.Refresh();
                // Todo: will result in many events, maybe one event for all of them?
                SelectionChanged?.Invoke(model, false);
            }

            _selectedModels.Clear();
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

        public void RegisterModelComponent<M, C>() where M : Model where C : ComponentBase
        {
            var modelType = typeof(M);
            if (_componentByModelMapping.ContainsKey(modelType))
                throw new Exception($"Component already registered for model '{modelType.Name}'.");

            _componentByModelMapping.Add(modelType, typeof(C));
        }

        public Type? GetComponentForModel<M>(M model) where M : Model
        {
            var modelType = model.GetType();
            return _componentByModelMapping.ContainsKey(modelType) ? _componentByModelMapping[modelType] : null;
        }

        public void Refresh() => Changed?.Invoke();

        internal void OnMouseDown(Model model, MouseEventArgs e) => MouseDown?.Invoke(model, e);

        internal void OnMouseMove(Model model, MouseEventArgs e) => MouseMove?.Invoke(model, e);

        internal void OnMouseUp(Model model, MouseEventArgs e) => MouseUp?.Invoke(model, e);

        internal void OnKeyDown(KeyboardEventArgs e) => KeyDown?.Invoke(e);

        internal void OnWheel(WheelEventArgs e) => Wheel?.Invoke(e);
    }
}
