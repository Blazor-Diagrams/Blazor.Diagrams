using Blazor.Diagrams.Core.Behaviors;
using Blazor.Diagrams.Core.Extensions;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Layers;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;
using Blazor.Diagrams.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Blazor.Diagrams")]
[assembly: InternalsVisibleTo("Blazor.Diagrams.Tests")]
[assembly: InternalsVisibleTo("Blazor.Diagrams.Core.Tests")]
namespace Blazor.Diagrams.Core
{
    public class DiagramBase : Model
    {
        private readonly Dictionary<Type, Behavior> _behaviors;
        private readonly List<GroupModel> _groups;

        public event Action<Model?, PointerEventArgs>? PointerDown;
        public event Action<Model?, PointerEventArgs>? PointerMove;
        public event Action<Model?, PointerEventArgs>? PointerUp;
        public event Action<Model?, PointerEventArgs>? PointerEnter;
        public event Action<Model?, PointerEventArgs>? PointerLeave;
        public event Action<KeyboardEventArgs>? KeyDown;
        public event Action<WheelEventArgs>? Wheel;
        public event Action<Model?, PointerEventArgs>? PointerClick;
        public event Action<Model?, PointerEventArgs>? PointerDoubleClick;

        public event Action<SelectableModel>? SelectionChanged;
        public event Action<GroupModel>? GroupAdded;
        public event Action<GroupModel>? GroupUngrouped;
        public event Action<GroupModel>? GroupRemoved;

        public event Action? PanChanged;
        public event Action? ZoomChanged;
        public event Action? ContainerChanged;

        public DiagramBase(DiagramOptions? options = null)
        {
            _behaviors = new Dictionary<Type, Behavior>();
            _groups = new List<GroupModel>();

            Options = options ?? new DiagramOptions();
            Nodes = new NodeLayer(this);
            Links = new LinkLayer(this);

            RegisterBehavior(new SelectionBehavior(this));
            RegisterBehavior(new DragMovablesBehavior(this));
            RegisterBehavior(new DragNewLinkBehavior(this));
            RegisterBehavior(new PanBehavior(this));
            RegisterBehavior(new ZoomBehavior(this));
            RegisterBehavior(new EventsBehavior(this));
            RegisterBehavior(new KeyboardShortcutsBehavior(this));
        }

        public NodeLayer Nodes { get; }
        public LinkLayer Links { get; }
        public IReadOnlyList<GroupModel> Groups => _groups;
        public Rectangle? Container { get; private set; }
        public Point Pan { get; private set; } = Point.Zero;
        public double Zoom { get; private set; } = 1;
        public DiagramOptions Options { get; }
        public bool SuspendRefresh { get; set; }

        public override void Refresh()
        {
            if (SuspendRefresh)
                return;

            base.Refresh();
        }

        public void Batch(Action action)
        {
            if (SuspendRefresh)
            {
                // If it's already suspended, just execute the action and leave it suspended
                // It's probably handled by an outer batch
                action();
                return;
            }

            SuspendRefresh = true;
            action();
            SuspendRefresh = false;
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
            var group = Options.Groups.Factory(this, children);
            AddGroup(group);
            return group;
        }

        /// <summary>
        /// Adds the group to the diagram after validating it.
        /// </summary>
        /// <param name="group">A group instance.</param>
        public GroupModel AddGroup(GroupModel group)
        {
            foreach (var child in group.Children)
            {
                if (child is GroupModel g)
                {
                    if (!Groups.Contains(g))
                        throw new Exception("One of the children isn't in the diagram (Groups). Make sure to add all the nodes before creating the group.");
                }
                else if (child is NodeModel n)
                    if (!Nodes.Contains(n))
                        throw new Exception("One of the children isn't in the diagram (Nodes). Make sure to add all the nodes before creating the group.");
            }

            _groups.Add(group);
            GroupAdded?.Invoke(group);
            Refresh();
            return group;
        }

        /// <summary>
        /// Splits up the group by deleting the group and keeping the children.
        /// </summary>
        /// <param name="group">A group instance.</param>
        public void Ungroup(GroupModel group)
        {
            if (!_groups.Remove(group))
                return;

            Batch(() =>
            {
                group.Ungroup();
                Links.Remove(group.PortLinks.ToArray());
                GroupUngrouped?.Invoke(group);
            });
        }

        /// <summary>
        /// Deletes the group and all its children from the diagram.
        /// </summary>
        /// <param name="group">A group instnace.</param>
        public void RemoveGroup(GroupModel group)
        {
            if (!_groups.Remove(group))
                return;

            Batch(() =>
            {
                Nodes.Remove(group.Children.ToArray());
                Links.Remove(group.PortLinks.ToArray());
                group.Ungroup();
                GroupRemoved?.Invoke(group);
            });
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

        public T? GetBehavior<T>() where T : Behavior
        {
            var type = typeof(T);
            return (T?)(_behaviors.ContainsKey(type) ? _behaviors[type] : null);
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

        public void ZoomToFit(double margin = 10)
        {
            if (Container == null || Nodes.Count == 0)
                return;

            var selectedNodes = Nodes.Where(s => s.Selected);
            var nodesToUse = selectedNodes.Any() ? selectedNodes : Nodes;
            var bounds = nodesToUse.GetBounds();
            var width = bounds.Width + 2 * margin;
            var height = bounds.Height + 2 * margin;
            var minX = bounds.Left - margin;
            var minY = bounds.Top - margin;

            SuspendRefresh = true;

            var xf = Container.Width / width;
            var yf = Container.Height / height;
            SetZoom(Math.Min(xf, yf));

            var nx = Container.Left + Pan.X + minX * Zoom;
            var ny = Container.Top + Pan.Y + minY * Zoom;
            UpdatePan(Container.Left - nx, Container.Top - ny);

            SuspendRefresh = false;
            Refresh();
        }

        public void SetPan(double x, double y)
        {
            Pan = new Point(x, y);
            PanChanged?.Invoke();
            Refresh();
        }

        public void UpdatePan(double deltaX, double deltaY)
        {
            Pan = Pan.Add(deltaX, deltaY);
            PanChanged?.Invoke();
            Refresh();
        }

        public void SetZoom(double newZoom)
        {
            if (newZoom <= 0)
                throw new ArgumentException($"{nameof(newZoom)} cannot be equal or lower than 0");

            if (newZoom < Options.Zoom.Minimum)
                newZoom = Options.Zoom.Minimum;

            Zoom = newZoom;
            ZoomChanged?.Invoke();
            Refresh();
        }

        public void SetContainer(Rectangle newRect)
        {
            if (newRect.Equals(Container))
                return;

            Container = newRect;
            ContainerChanged?.Invoke();
            Refresh();
        }

        public Point GetRelativeMousePoint(double clientX, double clientY)
        {
            if (Container == null)
                throw new Exception("Container not available. Make sure you're not using this method before the diagram is fully loaded");

            return new Point((clientX - Container.Left - Pan.X) / Zoom, (clientY - Container.Top - Pan.Y) / Zoom);
        }

        public Point GetRelativePoint(double clientX, double clientY)
        {
            if (Container == null)
                throw new Exception("Container not available. Make sure you're not using this method before the diagram is fully loaded");

            return new Point(clientX - Container.Left, clientY - Container.Top);
        }

        public Point GetScreenPoint(double clientX, double clientY)
        {
            if (Container == null)
                throw new Exception("Container not available. Make sure you're not using this method before the diagram is fully loaded");

            return new Point(Zoom * clientX + Container.Left + Pan.X, Zoom * clientY + Container.Top + Pan.Y);
        }

        #region Events

        internal void TriggerPointerDown(Model? model, PointerEventArgs e) => PointerDown?.Invoke(model, e);

        internal void TriggerPointerMove(Model? model, PointerEventArgs e) => PointerMove?.Invoke(model, e);

        internal void TriggerPointerUp(Model? model, PointerEventArgs e) => PointerUp?.Invoke(model, e);
        
        internal void TriggerPointerEnter(Model? model, PointerEventArgs e) => PointerEnter?.Invoke(model, e);
        
        internal void TriggerPointerLeave(Model? model, PointerEventArgs e) => PointerLeave?.Invoke(model, e);

        internal void OnKeyDown(KeyboardEventArgs e) => KeyDown?.Invoke(e);

        internal void OnWheel(WheelEventArgs e) => Wheel?.Invoke(e);

        internal void TriggerPointerClick(Model? model, PointerEventArgs e) => PointerClick?.Invoke(model, e);

        internal void TriggerPointerDoubleClick(Model? model, PointerEventArgs e) => PointerDoubleClick?.Invoke(model, e);

        #endregion
    }
}
