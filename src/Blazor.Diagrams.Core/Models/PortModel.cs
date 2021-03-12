using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models.Base;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Blazor.Diagrams.Core.Models
{
    public class PortModel : Model
    {
        private readonly List<BaseLinkModel> _links = new List<BaseLinkModel>(4);

        public PortModel(NodeModel parent, PortAlignment alignment = PortAlignment.Bottom, Point? position = null,
            Size? size = null)
        {
            Parent = parent;
            Alignment = alignment;
            Position = position ?? Point.Zero;
            Size = size ?? Size.Zero;
        }

        public PortModel(string id, NodeModel parent, PortAlignment alignment = PortAlignment.Bottom,
            Point? position = null, Size? size = null) : base(id)
        {
            Parent = parent;
            Alignment = alignment;
            Position = position ?? Point.Zero;
            Size = size ?? Size.Zero;
        }

        public NodeModel Parent { get; }
        public PortAlignment Alignment { get; }
        public Point Position { get; set; }
        public Point MiddlePosition => new Point(Position.X + Size.Width / 2, Position.Y + Size.Height / 2);
        public Size Size { get; set; }
        public ReadOnlyCollection<BaseLinkModel> Links => _links.AsReadOnly();
        public bool Initialized { get; internal set; }

        public void RefreshAll()
        {
            Refresh();
            RefreshLinks();
        }

        public void RefreshLinks() => _links.ForEach(l => l.Refresh());

        public T GetParent<T>() where T : NodeModel => (T)Parent;

        public virtual bool CanAttachTo(PortModel port)
            => port != this && !port.Locked && Parent != port.Parent;

        internal void AddLink(BaseLinkModel link) => _links.Add(link);

        internal void RemoveLink(BaseLinkModel link) => _links.Remove(link);
    }
}
