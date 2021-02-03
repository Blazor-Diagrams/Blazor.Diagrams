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
        public event Action<SelectableModel, bool>? SelectionChanged;
        public event Action<GroupModel>? GroupAdded;
        public event Action<GroupModel>? GroupUngrouped;
        public event Action<GroupModel>? GroupRemoved;

        public event Action? PanChanged;
        public event Action? ZoomChanged;
        public event Action? ContainerChanged;

        public DiagramManager(DiagramOptions? options = null)
        {
            _subManagers = new List<DiagramSubManager>();
            _componentByModelMapping = new Dictionary<Type, Type>();
            _selectedModels = new HashSet<SelectableModel>();
            _groups = new List<GroupModel>();

            Options = options ?? new DiagramOptions();
            Nodes = new Layer<NodeModel>();
            Links = new Layer<LinkModel>();

            Nodes.Added += OnNodesAdded;
            Nodes.Removed += OnNodesRemoved;
            Links.Added += OnLinksAdded;
            Links.Removed += OnLinksRemoved;

            RegisterSubManager<SelectionSubManager>();
            RegisterSubManager<DragMovablesSubManager>();
            RegisterSubManager<DragNewLinkSubManager>();
            RegisterSubManager<DeleteSelectionSubManager>();
            RegisterSubManager<PanSubManager>();
            RegisterSubManager<ZoomSubManager>();
            RegisterSubManager<GroupingSubManager>();
        }

        public Layer<NodeModel> Nodes { get; }
        public Layer<LinkModel> Links { get; }
        public IReadOnlyCollection<SelectableModel> SelectedModels => _selectedModels;
        //public IReadOnlyList<NodeModel> Nodes => _nodes;
        //public IEnumerable<LinkModel> Links => _nodes.SelectMany(n => n.AllLinks).Union(_groups.SelectMany(g => g.AllLinks)).Distinct();
        public IReadOnlyList<GroupModel> Groups => _groups;
        public Rectangle? Container { get; internal set; }
        public Point Pan { get; internal set; } = Point.Zero;
        public double Zoom { get; private set; } = 1;
        public DiagramOptions Options { get; }

        private void OnNodesAdded(NodeModel[] _) => Refresh();

        private void OnNodesRemoved(NodeModel[] nodes)
        {
            foreach (var node in nodes)
            {
                Links.Remove(node.AllLinks.ToArray());
            }

            Refresh();
        }

        private void OnLinksAdded(LinkModel[] links)
        {
            foreach (var link in links)
            {
                link.SourcePort.AddLink(link);
                link.TargetPort?.AddLink(link);

                link.SourcePort.Refresh();
                link.TargetPort?.Refresh();

                link.SourcePort.Parent.Group?.Refresh();
                link.TargetPort?.Parent.Group?.Refresh();
            }

            Refresh();
        }

        private void OnLinksRemoved(LinkModel[] links)
        {
            foreach (var link in links)
            {
                link.SourcePort.RemoveLink(link);
                link.TargetPort?.RemoveLink(link);

                link.SourcePort.Refresh();
                link.TargetPort?.Refresh();

                link.SourcePort.Parent.Group?.Refresh();
                link.TargetPort?.Parent.Group?.Refresh();
            }

            Refresh();
        }

        #region Groups

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
                if (child is NodeModel node && !Nodes.Contains(node))
                    throw new Exception("One of the nodes isn't in the diagram. Make sure to add all the nodes before creating the group.");
            }

            _groups.Add(group);
            GroupAdded?.Invoke(group);
            Refresh();
        }

        public void Ungroup(GroupModel group)
        {
            if (!_groups.Remove(group))
                return;

            // Todo: batch Refresh()
            group.Ungroup();
            Links.Remove(group.AllLinks.ToArray());
            GroupUngrouped?.Invoke(group);
            Refresh();
        }

        public void RemoveGroup(GroupModel group)
        {
            if (!_groups.Remove(group))
                return;

            // Todo: batch Refresh()
            group.Ungroup();
            Nodes.Remove(group.Children);
            Links.Remove(group.AllLinks.ToArray());
            GroupRemoved?.Invoke(group);
            Refresh();
        }

        #endregion

        #region Selection

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

        #endregion

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
            if (Nodes.Count == 0)
                return;

            var selectedNodes = SelectedModels.Where(s => s is NodeModel).Select(s => (NodeModel)s);
            (var minX, var maxX, var minY, var maxY) = GetNodesRect(selectedNodes.Any() ? selectedNodes : Nodes);
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
