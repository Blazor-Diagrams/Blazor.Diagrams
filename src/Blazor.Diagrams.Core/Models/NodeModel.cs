using Blazor.Diagrams.Core.Models.Base;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Blazor.Diagrams.Core.Models
{
    public class NodeModel<PortType> : SelectableModel where PortType : PortModel
    {
        private Size? _size;
        private readonly List<PortType> _ports = new List<PortType>();

        public NodeModel(Point? position = null, RenderLayer layer = RenderLayer.HTML)
        {
            Position = position ?? Point.Zero;
            Layer = layer;
        }

        public NodeModel(string id, Point? position = null, RenderLayer layer = RenderLayer.HTML) : base(id)
        {
            Position = position ?? Point.Zero;
            Layer = layer;
        }

        public Size? Size
        {
            get => _size;
            set
            {
                _size = value;
                Refresh();
            }
        }

        public Point Position { get; private set; }
        public RenderLayer Layer { get; }
        public ReadOnlyCollection<PortType> Ports => _ports.AsReadOnly();
        public IEnumerable<LinkModel> AllLinks => Ports.SelectMany(p => p.Links);
        public Group? Group { get; internal set; }

        public PortType AddPort(PortType port)
        {
            _ports.Add(port);
            return port;
        }

        public PortType GetPort(PortAlignment alignment) => Ports.FirstOrDefault(p => p.Alignment == alignment);

        public bool RemovePort(PortType port) => _ports.Remove(port);

        public void SetPosition(double x, double y)
        {
            Position = new Point(x, y);
            foreach (var port in _ports)
            {
                port.Position = new Point(Position.X + port.Offset.X, Position.Y + port.Offset.Y);
            }
        }

        public void RefreshAll()
        {
            Refresh();
            _ports.ForEach(p => p.RefreshAll());
        }
    }

    public class NodeModel : NodeModel<PortModel>
    {
        public NodeModel(Point? position = null, RenderLayer layer = RenderLayer.HTML) : base(position, layer) { }

        public NodeModel(string id, Point? position = null, RenderLayer layer = RenderLayer.HTML) : base(id, position, layer) { }

        public PortModel AddPort(PortAlignment alignment = PortAlignment.Bottom)
        {
            var port = new PortModel(this, alignment, Position);
            return AddPort(port);
        }
    }
}