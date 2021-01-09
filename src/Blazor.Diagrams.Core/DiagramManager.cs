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
        private readonly List<GroupModel> _groups;

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
        public event Action<GroupModel>? GroupAdded;
        public event Action<GroupModel>? GroupRemoved;

        public event Action? PanChanged;
        public event Action? ZoomChanged;
        public event Action? ContainerChanged;

        public DiagramManager(DiagramOptions? options = null)
        {
            _nodes = new List<NodeModel>();
            _subManagers = new List<DiagramSubManager>();
            _componentByModelMapping = new Dictionary<Type, Type>();
            _selectedModels = new HashSet<SelectableModel>();
            _groups = new List<GroupModel>();

            Options = options ?? new DiagramOptions();

            RegisterSubManager<SelectionSubManager>();
            RegisterSubManager<DragMovablesSubManager>();
            RegisterSubManager<DragNewLinkSubManager>();
            RegisterSubManager<DeleteSelectionSubManager>();
            RegisterSubManager<PanSubManager>();
            RegisterSubManager<ZoomSubManager>();
            RegisterSubManager<GroupingSubManager>();
        }

        public IReadOnlyCollection<NodeModel> Nodes => _nodes;
        public IEnumerable<LinkModel> AllLinks => _nodes.SelectMany(n => n.AllLinks).Union(_groups.SelectMany(g => g.AllLinks)).Distinct();
        public IReadOnlyCollection<SelectableModel> SelectedModels => _selectedModels;
        public IReadOnlyCollection<GroupModel> Groups => _groups;
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

        /// <summary>
        /// Creates and configures an instance of a link model that expects a constructor with two parameters:
        /// the source port and the target port.
        /// </summary>
        /// <returns>The created link instance.</returns>
        public T AddLink<T>(PortModel source, PortModel? target = null) where T : LinkModel
        {
            var link = (T)Activator.CreateInstance(typeof(T), source, target);
            return AddLink(link, source, target);
        }

        public T AddLink<T>(T link, PortModel source, PortModel? target = null) where T : LinkModel
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

            source.Parent.Group?.Refresh();
            target?.Parent.Group?.Refresh();

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
            link.Refresh();
            targetPort.Refresh();

            link.SourcePort.Parent.Group?.Refresh();
            targetPort?.Parent.Group?.Refresh();

            LinkAttached?.Invoke(link);
        }

        public void RemoveLink(LinkModel link, bool triggerEvent = true)
        {
            link.SourcePort.RemoveLink(link);
            link.TargetPort?.RemoveLink(link);

            LinkRemoved?.Invoke(link);
            link.SourcePort.Refresh();
            link.TargetPort?.Refresh();

            link.SourcePort.Parent.Group?.Refresh();
            link.TargetPort?.Parent.Group?.Refresh();

            if (triggerEvent)
            {
                Changed?.Invoke();
            }
        }

        /// <summary>
        /// Groups 2 or more children.
        /// </summary>
        /// <param name="children">An array of child nodes.</param>
        /// <returns>The created group instance.</returns>
        public GroupModel Group(params NodeModel[] children)
        {
            if (children.Any(n => n.Group != null))
                throw new InvalidOperationException("Cannot group nodes that already belong to another group");

            var group = new GroupModel(this, children);
            AddGroup(group);
            return group;
        }

        /// <summary>
        /// Adds the group to the diagram after validating it.
        /// </summary>
        /// <param name="group">A group instance.</param>
        public void AddGroup(GroupModel group)
        {
            if (group.Children.Length < 2)
                throw new ArgumentException("Number of nodes must be >= 2");

            var layers = group.Children.Select(n => n.Layer).Distinct();
            if (layers.Count() > 1)
                throw new InvalidOperationException("Cannot group nodes with different layers");

            if (layers.First() == RenderLayer.SVG)
                throw new InvalidOperationException("SVG groups aren't implemented yet");

            foreach (var child in group.Children)
            {
                if (child is GroupModel g)
                {
                    _groups.Remove(g);
                }
                else
                {
                    _nodes.Remove(child);
                }
            }

            _groups.Add(group);
            GroupAdded?.Invoke(group);
            Refresh();
        }

        public void Ungroup(GroupModel group)
        {
            if (!_groups.Remove(group))
                return;

            group.Ungroup();

            foreach (var child in group.Children)
            {
                if (child is GroupModel g)
                {
                    _groups.Add(g);
                }
                else
                {
                    _nodes.Add(child);
                }
            }

            GroupRemoved?.Invoke(group);
            Refresh();
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
            => RegisterModelComponent(typeof(M), typeof(C));

        public void RegisterModelComponent(Type modelType, Type componentType)
        {
            if (_componentByModelMapping.ContainsKey(modelType))
                throw new Exception($"Component already registered for model '{modelType.Name}'.");

            _componentByModelMapping.Add(modelType, componentType);
        }

        public Type? GetComponentForModel<M>(M model) where M : Model
        {
            var modelType = model.GetType();
            return _componentByModelMapping.ContainsKey(modelType) ? _componentByModelMapping[modelType] : null;
        }

        public void Refresh() => Changed?.Invoke();

        public void ZoomToFit(double margin = 10)
        {
            if (_nodes.Count == 0)
                return;

            var selectedNodes = SelectedModels.Where(s => s is NodeModel).Select(s => (NodeModel)s);
            (var minX, var maxX, var minY, var maxY) = GetNodesRect(selectedNodes.Any() ? selectedNodes : _nodes);
            var width = maxX - minX + 2 * margin;
            var height = maxY - minY + 2 * margin;
            minX -= margin;
            minY -= margin;

            var xf = Container.Width / width;
            var yf = Container.Height / height;
            Zoom = Math.Min(xf, yf);

            var nx = Container.Left + Pan.X + minX * Zoom;
            var ny = Container.Top + Pan.Y + minY * Zoom;
            UpdatePan(Container.Left - nx, Container.Top - ny);

            Refresh();
        }

        public (double minX, double maxX, double minY, double maxY) GetNodesRect(IEnumerable<NodeModel> nodes)
        {
            var minX = double.MaxValue;
            var maxX = double.MinValue;
            var minY = double.MaxValue;
            var maxY = double.MinValue;

            foreach (var node in nodes)
            {
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

        public void UpdatePan(double deltaX, double deltaY)
        {
            Pan = Pan.Add(deltaX, deltaY);
            PanChanged?.Invoke();
            Refresh();
        }

        public void SetZoom(double newZoom)
        {
            Zoom = newZoom;
            ZoomChanged?.Invoke();
            Refresh();
        }

        internal void SetContainer(Rectangle newRect)
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
