using Blazor.Diagrams.Core.Models.Base;
using Blazor.Diagrams.Core.Models.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Blazor.Diagrams.Core.Models
{
    public class NodeModel : MovableModel
    {
        private readonly List<PortModel> _ports = new List<PortModel>();
        private Size? _size;

        public event Action<NodeModel>? SizeChanged;
        public event Action<NodeModel>? Moving;

        public NodeModel(Point? position = null, RenderLayer layer = RenderLayer.HTML) : base(position)
        {
            Layer = layer;
        }

        public NodeModel(string id, Point? position = null, RenderLayer layer = RenderLayer.HTML) : base(id, position)
        {
            Layer = layer;
        }

        public RenderLayer Layer { get; }
        public Size? Size
        {
            get => _size;
            set
            {
                if (value?.Equals(_size) == true)
                    return;

                _size = value;
                SizeChanged?.Invoke(this);
            }
        }
        public GroupModel? Group { get; internal set; }

        public ReadOnlyCollection<PortModel> Ports => _ports.AsReadOnly();
        public IEnumerable<BaseLinkModel> AllLinks => Ports.SelectMany(p => p.Links);

        public PortModel AddPort(PortModel port)
        {
            _ports.Add(port);
            return port;
        }

        public PortModel AddPort(PortAlignment alignment = PortAlignment.Bottom)
            => AddPort(new PortModel(this, alignment, Position));

        public PortModel GetPort(PortAlignment alignment) => Ports.FirstOrDefault(p => p.Alignment == alignment);

        public T GetPort<T>(PortAlignment alignment) where T : PortModel => (T)GetPort(alignment);

        public void RefreshAll()
        {
            Refresh();
            _ports.ForEach(p => p.RefreshAll());
        }

        public void ReinitializePorts()
        {
            foreach (var port in Ports)
            {
                port.Initialized = false;
                port.Refresh();
            }
        }

        public bool RemovePort(PortModel port) => _ports.Remove(port);

        public override void SetPosition(double x, double y)
        {
            var deltaX = x - Position.X;
            var deltaY = y - Position.Y;
            base.SetPosition(x, y);

            UpdatePortPositions(deltaX, deltaY);
            Refresh();
            Moving?.Invoke(this);
        }

        public virtual void UpdatePositionSilently(double deltaX, double deltaY)
        {
            base.SetPosition(Position.X + deltaX, Position.Y + deltaY);
            UpdatePortPositions(deltaX, deltaY);
            Refresh();
        }

        public Rectangle? GetBounds(bool includePorts = false)
        {
            if (Size == null)
                return null;

            if (!includePorts)
                return new Rectangle(Position, Size);

            var leftPort = GetPort(PortAlignment.Left);
            var topPort = GetPort(PortAlignment.Top);
            var rightPort = GetPort(PortAlignment.Right);
            var bottomPort = GetPort(PortAlignment.Bottom);

            var left = leftPort == null ? Position.X : Math.Min(Position.X, leftPort.Position.X);
            var top = topPort == null ? Position.Y : Math.Min(Position.Y, topPort.Position.Y);
            var right = rightPort == null ? Position.X + Size!.Width :
                Math.Max(rightPort.Position.X + rightPort.Size.Width, Position.X + Size!.Width);
            var bottom = bottomPort == null ? Position.Y + Size!.Height :
                Math.Max(bottomPort.Position.Y + bottomPort.Size.Height, Position.Y + Size!.Height);

            return new Rectangle(left, top, right, bottom);
        }

        private void UpdatePortPositions(double deltaX, double deltaY)
        {
            // Save some JS calls and update ports directly here
            foreach (var port in _ports)
            {
                port.Position = new Point(port.Position.X + deltaX, port.Position.Y + deltaY);
                port.RefreshLinks();
            }
        }
    }
}