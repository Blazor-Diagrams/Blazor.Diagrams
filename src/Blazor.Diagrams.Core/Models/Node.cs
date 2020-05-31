using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Blazor.Diagrams.Core.Models
{
    public class Node : Model
    {
        private List<Port> _ports = new List<Port>();

        public Node(Point position = null)
        {
            Position = position ?? Point.Zero;
        }

        public Node(string id, Point position = null) : base(id)
        {
            Position = position ?? Point.Zero;
        }

        public Point LastOffset { get; set; }
        public Point Position { get; set; }
        public ReadOnlyCollection<Port> Ports => _ports.AsReadOnly();
        public bool Selected { get; set; }

        public Port AddPort(PortAlignment alignment = PortAlignment.BOTTOM)
        {
            var port = new Port(alignment, Position);
            _ports.Add(port);
            return port;
        }

        public Port GetPort(PortAlignment alignment) => Ports.FirstOrDefault(p => p.Alignment == alignment);

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
