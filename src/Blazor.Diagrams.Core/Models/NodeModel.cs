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
        public Size? Size { get; set; }
        public GroupModel? Group { get; internal set; }

        public ReadOnlyCollection<PortModel> Ports => _ports.AsReadOnly();
        public IEnumerable<LinkModel> AllLinks => Ports.SelectMany(p => p.Links);

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

        public bool RemovePort(PortModel port) => _ports.Remove(port);

        public override void SetPosition(double x, double y)
        {
            var deltaX = x - Position.X;
            var deltaY = y - Position.Y;
            base.SetPosition(x, y);

            // Save some JS calls and update ports directly here
            foreach (var port in _ports)
            {
                port.Position = new Point(port.Position.X + deltaX, port.Position.Y + deltaY);
            }

            RefreshAll();
            Moving?.Invoke(this);
        }

        public virtual void UpdatePositionSilently(double deltaX, double deltaY)
        {
            base.SetPosition(Position.X + deltaX, Position.Y + deltaY);

            // Save some JS calls and update ports directly here
            foreach (var port in _ports)
            {
                port.Position = new Point(port.Position.X + deltaX, port.Position.Y + deltaY);
            }

            RefreshAll();
        }
    }
}