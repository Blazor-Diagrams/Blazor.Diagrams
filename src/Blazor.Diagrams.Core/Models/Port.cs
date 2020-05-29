using System.Collections.Generic;

namespace Blazor.Diagrams.Core.Models
{
    public class Port : Model
    {
        public Port(PortAlignment alignment = PortAlignment.BOTTOM, Point position = null)
        {
            Alignment = alignment;
            Position = position ?? Point.Zero;
        }

        public Port(string id, PortAlignment alignment = PortAlignment.BOTTOM, Point position = null) : base(id)
        {
            Alignment = alignment;
            Position = position ?? Point.Zero;
        }

        public PortAlignment Alignment { get; }
        public Point Offset { get; set; }
        public Point Position { get; set; }
        public List<Link> Links { get; } = new List<Link>();

        public Link AddLink(Port targetPort)
        {
            var link = new Link(this, targetPort);
            AddLink(link);
            targetPort.AddLink(link);
            return link;
        }

        internal void AddLink(Link link) => Links.Add(link);

        public void RefreshAll()
        {
            Refresh();
            Links.ForEach(l => l.Refresh());
        }
    }
}
