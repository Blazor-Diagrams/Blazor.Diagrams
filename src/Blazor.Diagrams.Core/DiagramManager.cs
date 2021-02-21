using Blazor.Diagrams.Core.Behaviors;
using Blazor.Diagrams.Core.Extensions;
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
        private readonly Dictionary<Type, Behavior> _behaviors;
        private readonly Dictionary<Type, Type> _componentByModelMapping;
        private readonly List<GroupModel> _groups;

        public event Action<Model, MouseEventArgs>? MouseDown;
        public event Action<Model, MouseEventArgs>? MouseMove;
        public event Action<Model, MouseEventArgs>? MouseUp;
        public event Action<KeyboardEventArgs>? KeyDown;
        public event Action<WheelEventArgs>? Wheel;
        public event Action<Model, MouseEventArgs>? MouseClick;

        public event Action? Changed;
        public event Action<SelectableModel>? SelectionChanged;
        public event Action<GroupModel>? GroupAdded;
        public event Action<GroupModel>? GroupUngrouped;
        public event Action<GroupModel>? GroupRemoved;

        public event Action? PanChanged;
        public event Action? ZoomChanged;
        public event Action? ContainerChanged;

        public DiagramManager(DiagramOptions? options = null)
        {
            _behaviors = new Dictionary<Type, Behavior>();
            _componentByModelMapping = new Dictionary<Type, Type>();
            _groups = new List<GroupModel>();

            Options = options ?? new DiagramOptions();
            Nodes = new Layer<NodeModel>();
            Links = new Layer<BaseLinkModel>();

            Nodes.Added += OnNodesAdded;
            Nodes.Removed += OnNodesRemoved;
            Links.Added += OnLinksAdded;
            Links.Removed += OnLinksRemoved;

            RegisterBehavior(new SelectionBehavior(this));
            RegisterBehavior(new DragMovablesBehavior(this));
            RegisterBehavior(new DragNewLinkBehavior(this));
            RegisterBehavior(new DeleteSelectionBehavior(this));
            RegisterBehavior(new PanBehavior(this));
            RegisterBehavior(new ZoomBehavior(this));
            RegisterBehavior(new GroupingBehavior(this));
            RegisterBehavior(new EventsBehavior(this));
        }

        public Layer<NodeModel> Nodes { get; }
        public Layer<BaseLinkModel> Links { get; }
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

        private void OnLinksAdded(BaseLinkModel[] links)
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

        private void OnLinksRemoved(BaseLinkModel[] links)
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

            var group = Options.Groups.Factory(this, children);
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

        public IEnumerable<SelectableModel> GetSelectedModels()
        {
            foreach (var node in Nodes)
            {
                if (node.Selected)
                    yield return node;
            }

            foreach (var link in Links)
            {
                if (link.Selected)
                    yield return link;

                foreach (var vertex in link.Vertices)
                {
                    if (vertex.Selected)
                        yield return vertex;
                }    
            }

            foreach (var group in Groups)
            {
                if (group.Selected)
                    yield return group;
            }
        }

        public void SelectModel(SelectableModel model, bool unselectOthers)
        {
            if (model.Selected)
                return;

            if (unselectOthers)
                UnselectAll();

            model.Selected = true;
            model.Refresh();
            SelectionChanged?.Invoke(model);
        }

        public void UnselectModel(SelectableModel model)
        {
            if (!model.Selected)
                return;

            model.Selected = false;
            model.Refresh();
            SelectionChanged?.Invoke(model);
        }

        public void UnselectAll()
        {
            foreach (var model in GetSelectedModels())
            {
                model.Selected = false;
                model.Refresh();
                // Todo: will result in many events, maybe one event for all of them?
                SelectionChanged?.Invoke(model);
            }
        }

        #endregion

        #region Behaviors

        public void RegisterBehavior(Behavior behavior)
        {
            var type = behavior.GetType();
            if (_behaviors.ContainsKey(type))
                throw new Exception($"Behavior '{type.Name}' already registered");

            _behaviors.Add(type, behavior);
        }

        public void UnregisterBehavior<T>() where T : Behavior
        {
            var type = typeof(T);
            if (!_behaviors.ContainsKey(type))
                return;

            _behaviors[type].Dispose();
            _behaviors.Remove(type);
        }

        #endregion

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
            if (Container == null || Nodes.Count == 0)
                return;

            var selectedNodes = Nodes.Where(s => s.Selected);
            var nodesToUse = selectedNodes.Any() ? selectedNodes : Nodes;
            (var minX, var maxX, var minY, var maxY) = nodesToUse.GetBounds();
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
        {
            if (Container == null)
                throw new Exception("Container not available. Make sure you're not using this method before the diagram is fully loaded");

            return new Point((clientX - Container.Left - Pan.X) / Zoom, (clientY - Container.Top - Pan.Y) / Zoom);
        }

        internal void OnMouseDown(Model model, MouseEventArgs e) => MouseDown?.Invoke(model, e);

        internal void OnMouseMove(Model model, MouseEventArgs e) => MouseMove?.Invoke(model, e);

        internal void OnMouseUp(Model model, MouseEventArgs e) => MouseUp?.Invoke(model, e);

        internal void OnKeyDown(KeyboardEventArgs e) => KeyDown?.Invoke(e);

        internal void OnWheel(WheelEventArgs e) => Wheel?.Invoke(e);

        internal void OnMouseClick(Model model, MouseEventArgs e) => MouseClick?.Invoke(model, e);
    }
}
