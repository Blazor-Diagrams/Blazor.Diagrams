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
using Blazor.Diagrams.Core.Options;
using Blazor.Diagrams.Core.Controls;

[assembly: InternalsVisibleTo("Blazor.Diagrams")]
[assembly: InternalsVisibleTo("Blazor.Diagrams.Tests")]
[assembly: InternalsVisibleTo("Blazor.Diagrams.Core.Tests")]

namespace Blazor.Diagrams.Core
{
    public abstract class Diagram
    {
        private readonly Dictionary<Type, Behavior> _behaviors;

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
        public event Action? PanChanged;
        public event Action? ZoomChanged;
        public event Action? ContainerChanged;
        public event Action? Changed;

        protected Diagram()
        {
            _behaviors = new Dictionary<Type, Behavior>();

            Nodes = new NodeLayer(this);
            Links = new LinkLayer(this);
            Groups = new GroupLayer(this);
            Controls = new ControlsLayer();

            RegisterBehavior(new SelectionBehavior(this));
            RegisterBehavior(new DragMovablesBehavior(this));
            RegisterBehavior(new DragNewLinkBehavior(this));
            RegisterBehavior(new PanBehavior(this));
            RegisterBehavior(new ZoomBehavior(this));
            RegisterBehavior(new EventsBehavior(this));
            RegisterBehavior(new KeyboardShortcutsBehavior(this));
            RegisterBehavior(new ControlsBehavior(this));
            RegisterBehavior(new VirtualizationBehavior(this));
        }

        public abstract DiagramOptions Options { get; }
        public NodeLayer Nodes { get; }
        public LinkLayer Links { get; }
        public GroupLayer Groups { get; }
        public ControlsLayer Controls { get; }
        public Rectangle? Container { get; private set; }
        public Point Pan { get; private set; } = Point.Zero;
        public double Zoom { get; private set; } = 1;
        public bool SuspendRefresh { get; set; }

        public void Refresh()
        {
            if (SuspendRefresh)
                return;

            Changed?.Invoke();
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
                throw new Exception(
                    "Container not available. Make sure you're not using this method before the diagram is fully loaded");

            return new Point((clientX - Container.Left - Pan.X) / Zoom, (clientY - Container.Top - Pan.Y) / Zoom);
        }

        public Point GetRelativePoint(double clientX, double clientY)
        {
            if (Container == null)
                throw new Exception(
                    "Container not available. Make sure you're not using this method before the diagram is fully loaded");

            return new Point(clientX - Container.Left, clientY - Container.Top);
        }

        public Point GetScreenPoint(double clientX, double clientY)
        {
            if (Container == null)
                throw new Exception(
                    "Container not available. Make sure you're not using this method before the diagram is fully loaded");

            return new Point(Zoom * clientX + Container.Left + Pan.X, Zoom * clientY + Container.Top + Pan.Y);
        }

        #region Events

        public void TriggerPointerDown(Model? model, PointerEventArgs e) => PointerDown?.Invoke(model, e);

        public void TriggerPointerMove(Model? model, PointerEventArgs e) => PointerMove?.Invoke(model, e);

        public void TriggerPointerUp(Model? model, PointerEventArgs e) => PointerUp?.Invoke(model, e);

        public void TriggerPointerEnter(Model? model, PointerEventArgs e) => PointerEnter?.Invoke(model, e);

        public void TriggerPointerLeave(Model? model, PointerEventArgs e) => PointerLeave?.Invoke(model, e);

        public void TriggerKeyDown(KeyboardEventArgs e) => KeyDown?.Invoke(e);

        public void TriggerWheel(WheelEventArgs e) => Wheel?.Invoke(e);

        public void TriggerPointerClick(Model? model, PointerEventArgs e) => PointerClick?.Invoke(model, e);

        public void TriggerPointerDoubleClick(Model? model, PointerEventArgs e) => PointerDoubleClick?.Invoke(model, e);

        #endregion
    }
}