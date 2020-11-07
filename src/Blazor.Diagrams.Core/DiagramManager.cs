using Blazor.Diagrams.Core.Default;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;
using Blazor.Diagrams.Core.Models.Core;
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
        private readonly List<Group> _groups;

        public event Action<Model, MouseEventArgs>? MouseDown;
        public event Action<Model, MouseEventArgs>? MouseMove;
        public event Action<Model, MouseEventArgs>? MouseUp;
        public event Action<KeyboardEventArgs>? KeyDown;
        public event Action<WheelEventArgs>? Wheel;

        public event Action? Changed;
        public event Action<NodeModel>? NodeAdded;
        public event Action<NodeModel>? NodeRemoved;
        public event Action<SelectableModel, bool>? SelectionChanged;
        public event Action<LinkModel>? LinkAdded;
        public event Action<LinkModel>? LinkAttached;
        public event Action<LinkModel>? LinkRemoved;
        public event Action<Group>? GroupAdded;
        public event Action<Group>? GroupRemoved;

        public event Action? PanChanged;
        public event Action? ZoomChanged;
        public event Action? ContainerChanged;

        public DiagramManager(DiagramOptions? options = null)
        {
            _nodes = new List<NodeModel>();
            _subManagers = new List<DiagramSubManager>();
            _componentByModelMapping = new Dictionary<Type, Type>();
            _selectedModels = new HashSet<SelectableModel>();
            _groups = new List<Group>();

            Options = options ?? new DiagramOptions();

            RegisterSubManager<SelectionSubManager>();
            RegisterSubManager<DragNodeSubManager>();
            RegisterSubManager<DragNewLinkSubManager>();
            RegisterSubManager<DeleteSelectionSubManager>();
            RegisterSubManager<PanSubManager>();
            RegisterSubManager<ZoomSubManager>();
            RegisterSubManager<GroupingSubManager>();
        }

        public IReadOnlyCollection<NodeModel> Nodes => _nodes;
        public IEnumerable<LinkModel> AllLinks => _nodes.SelectMany(n => n.AllLinks).Distinct();
        public IReadOnlyCollection<SelectableModel> SelectedModels => _selectedModels;
        public IReadOnlyCollection<Group> Groups => _groups;
        public Rectangle? Container { get; internal set; }
        public Point Pan { get; internal set; } = Point.Zero;
        public double Zoom { get; private set; } = 1;
        public DiagramOptions Options { get; }

        public void AddNode(NodeModel node)
        {
            _nodes.Add(node);
            NodeAdded?.Invoke(node);
            Changed?.Invoke();
        }

        public void AddNodes(params NodeModel[] nodes)
        {
            _nodes.AddRange(nodes);

            // Is this okay?
            foreach (var node in nodes)
                NodeAdded?.Invoke(node);

            Changed?.Invoke();
        }

        public void RemoveNode(NodeModel node, bool triggerEvent = true)
        {
            if (_nodes.Remove(node))
            {
                // In case its selected
                _selectedModels.Remove(node);

                foreach (var link in node.AllLinks.ToList()) // Since we're removing from the list
                {
                    RemoveLink(link, false);
                }

                NodeRemoved?.Invoke(node);
                if (triggerEvent)
                {
                    Changed?.Invoke();
                }
            }
        }

        public LinkModel AddLink(PortModel source, PortModel? target = null)
        {
            if (Options?.Links?.DefaultLinkModel != null)
            {
                var link = (LinkModel)Activator.CreateInstance(Options?.Links?.DefaultLinkModel, source, target);
                return AddLink(link, source, target);
            }

            return AddLink<LinkModel>(source, target);
        }

        public T AddLink<T>(PortModel source, PortModel? target = null) where T : LinkModel
        {
            var link = (T)Activator.CreateInstance(typeof(T), source, target);
            return AddLink(link, source, target);
        }

        private T AddLink<T>(T link, PortModel source, PortModel? target = null) where T : LinkModel
        {
            link.Type = Options.Links.DefaultLinkType;
            source.AddLink(link);

            if (target == null)
            {
                link.OnGoingPosition = new Point(source.Position.X + source.Size.Width / 2,
                    source.Position.Y + source.Size.Height / 2);
            }
            else
            {
                target.AddLink(link);
            }

            source.Refresh();
            target?.Refresh();
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
            targetPort.Refresh();
            LinkAttached?.Invoke(link);
        }

        public void RemoveLink(LinkModel link, bool triggerEvent = true)
        {
            link.SourcePort.RemoveLink(link);
            link.TargetPort?.RemoveLink(link);

            LinkRemoved?.Invoke(link);
            link.SourcePort.Refresh();
            link.TargetPort?.Refresh();

            if (triggerEvent)
            {
                Changed?.Invoke();
            }
        }

        public Group Group(params NodeModel[] nodes)
        {
            if (nodes == null || nodes.Length == 0)
                throw new ArgumentException($"Null or empty nodes array");

            if (nodes.Select(n => n.Layer).Distinct().Count() > 1)
                throw new InvalidOperationException("Cannot group nodes with different layers");

            if (nodes.Any(n => n.Group != null))
                throw new InvalidOperationException("Cannot group nodes that already belong to another group");

            var group = new Group(nodes[0].Layer);

            foreach (var node in nodes)
            {
                node.Group = group;
                group.AddNode(node);
                node.Refresh();
            }

            _groups.Add(group);
            GroupAdded?.Invoke(group);
            return group;
        }

        public void Ungroup(Group group)
        {
            if (!_groups.Remove(group))
                return;

            foreach (var node in group.Nodes)
            {
                node.Group = null;
                node.Refresh();
            }

            group.Clear();
            GroupRemoved?.Invoke(group);
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
            foreach (var model in _selectedModels.ToList())
            {
                if (!model.Selected) // In case it got unselected by something else, like grouping
                    continue;

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

        public void ZoomToFit(double margin = 10)
        {
            var selectedNodes = SelectedModels.Where(s => s is NodeModel).Select(s => (NodeModel)s).ToList();
            if (selectedNodes.Count == 0 && _nodes.Count == 0)
                return;

            (var minX, var maxX, var minY, var maxY) = GetNodesRect(selectedNodes);
            var width = maxX - minX + 2 * margin;
            var height = maxY - minY + 2 * margin;
            minX -= margin;
            minY -= margin;

            var xf = Container.Width / width;
            var yf = Container.Height / height;
            Zoom = Math.Min(xf, yf);

            var nx = Container.Left + Pan.X + minX * Zoom;
            var ny = Container.Top + Pan.Y + minY * Zoom;
            Pan = Pan.Add(Container.Left - nx, Container.Top - ny);

            Refresh();
        }

        public (double minX, double maxX, double minY, double maxY) GetNodesRect(List<NodeModel>? nodes = null)
        {
            if (nodes == null || nodes.Count == 0)
                nodes = _nodes;

            if (nodes.Count == 0)
                return (0, 0, 0, 0);

            double minX = nodes[0].Position.X;
            double maxX = nodes[0].Position.X + nodes[0].Size!.Width;
            double minY = nodes[0].Position.Y;
            double maxY = nodes[0].Position.Y + nodes[0].Size!.Height;

            for (var i = 1; i < nodes.Count; i++)
            {
                var node = nodes[i];
                var trX = node.Position.X + node.Size!.Width;
                var bY = node.Position.Y + node.Size.Height;

                if (node.Position.X < minX)
                {
                    minX = node.Position.X;
                }
                if (trX > maxX)
                {
                    maxX = trX;
                }
                if (node.Position.Y < minY)
                {
                    minY = node.Position.Y;
                }
                if (bY > maxY)
                {
                    maxY = bY;
                }
            }

            return (minX, maxX, minY, maxY);
        }

        public void ChangePan(double deltaX, double deltaY)
        {
            Pan = Pan.Add(deltaX, deltaY);
            PanChanged?.Invoke();
            Refresh();
        }

        public void ChangeZoom(double newZoom)
        {
            Zoom = newZoom;
            ZoomChanged?.Invoke();
            Refresh();
        }

        internal void ChangeContainer(Rectangle newRect)
        {
            Container = newRect;
            ContainerChanged?.Invoke();
            Refresh();
        }

        public Point GetRelativePoint(double clientX, double clientY)
            => new Point((clientX - Container.Left - Pan.X) / Zoom, (clientY - Container.Top - Pan.Y) / Zoom);

        internal void OnMouseDown(Model model, MouseEventArgs e) => MouseDown?.Invoke(model, e);

        internal void OnMouseMove(Model model, MouseEventArgs e) => MouseMove?.Invoke(model, e);

        internal void OnMouseUp(Model model, MouseEventArgs e) => MouseUp?.Invoke(model, e);

        internal void OnKeyDown(KeyboardEventArgs e) => KeyDown?.Invoke(e);

        internal void OnWheel(WheelEventArgs e) => Wheel?.Invoke(e);
    }
}
