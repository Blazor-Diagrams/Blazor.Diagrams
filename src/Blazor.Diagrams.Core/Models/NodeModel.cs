using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Blazor.Diagrams.Core.Models
{
    public class NodeModel : Model
    {
        private List<PortModel> _ports = new List<PortModel>();

        public NodeModel(Point position = null)
        {
            Position = position ?? Point.Zero;
        }

        public NodeModel(string id, Point position = null) : base(id)
        {
            Position = position ?? Point.Zero;
        }

        public Point LastOffset { get; set; }
        public Point Position { get; set; }
        public ReadOnlyCollection<PortModel> Ports => _ports.AsReadOnly();
        public bool Selected { get; set; }

        public PortModel AddPort(PortAlignment alignment = PortAlignment.BOTTOM)
        {
            var port = new PortModel(alignment, Position);
            _ports.Add(port);
            return port;
        }

        public PortModel GetPort(PortAlignment alignment) => Ports.FirstOrDefault(p => p.Alignment == alignment);

        public void UpdatePosition(double clientX, double clientY)
        {
            Position = new Point(clientX + LastOffset.X, clientY + LastOffset.Y);
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
}
