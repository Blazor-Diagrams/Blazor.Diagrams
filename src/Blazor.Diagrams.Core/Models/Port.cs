using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Blazor.Diagrams.Core.Models
{
    public class Port : Model
    {
        private List<Link> _links = new List<Link>(4);

        public Port(PortAlignment alignment = PortAlignment.BOTTOM, Point position = null, Size size = null)
        {
            Alignment = alignment;
            Position = position ?? Point.Zero;
            Size = size ?? Size.Zero;
        }

        public Port(string id, PortAlignment alignment = PortAlignment.BOTTOM, Point position = null,
            Size size = null) : base(id)
        {
            Alignment = alignment;
            Position = position ?? Point.Zero;
            Size = size ?? Size.Zero;
        }

        public PortAlignment Alignment { get; }
        public Point Offset { get; set; }
        public Point Position { get; set; }
        public Size Size { get; set; }
        public ReadOnlyCollection<Link> Links => _links.AsReadOnly();

        public void RefreshAll()
        {
            Refresh();
            _links.ForEach(l => l.Refresh());
        }

        internal void AddLink(Link link) => _links.Add(link);
    }
}
