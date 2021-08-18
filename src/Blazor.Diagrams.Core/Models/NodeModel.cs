﻿using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Blazor.Diagrams.Core.Models
{
    public class NodeModel : MovableModel
    {
        private readonly List<PortModel> _ports = new List<PortModel>();
        private readonly List<BaseLinkModel> _links = new List<BaseLinkModel>();
        private Size? _size;

        public event Action<NodeModel>? SizeChanged;
        public event Action<NodeModel>? Moving;

        public NodeModel(Point? position = null, RenderLayer layer = RenderLayer.HTML,
            ShapeDefiner? shape = null) : base(position)
        {
            Layer = layer;
            ShapeDefiner = shape ?? Shapes.Rectangle;
        }

        public NodeModel(string id, Point? position = null, RenderLayer layer = RenderLayer.HTML,
            ShapeDefiner? shape = null) : base(id, position)
        {
            Layer = layer;
            ShapeDefiner = shape ?? Shapes.Rectangle;
        }

        public RenderLayer Layer { get; }
        public ShapeDefiner ShapeDefiner { get; }
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

        public IReadOnlyList<PortModel> Ports => _ports;
        public IReadOnlyList<BaseLinkModel> Links => _links;
        public IEnumerable<BaseLinkModel> AllLinks => Ports.SelectMany(p => p.Links);

        public string Label
        {
            get { return LabelModel.Content; }
            set { LabelModel.Content = value; }
        }

        NodeLabelModel _LabelModel;
        public NodeLabelModel LabelModel
        {
            get
            {
                if (_LabelModel == null)
                {
                    _LabelModel = new NodeLabelModel(this, "Node Title");
                }
                return _LabelModel;
            }
            set { _LabelModel = value; }
        }

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

        public void RefreshLinks()
        {
            foreach (var link in Links)
            {
                link.Refresh();
            }
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
            RefreshLinks();
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

        public IShape GetShape() => ShapeDefiner(this);

        private void UpdatePortPositions(double deltaX, double deltaY)
        {
            // Save some JS calls and update ports directly here
            foreach (var port in _ports)
            {
                port.Position = new Point(port.Position.X + deltaX, port.Position.Y + deltaY);
                port.RefreshLinks();
            }
        }

        internal void AddLink(BaseLinkModel link) => _links.Add(link);

        internal void RemoveLink(BaseLinkModel link) => _links.Remove(link);
    }
}