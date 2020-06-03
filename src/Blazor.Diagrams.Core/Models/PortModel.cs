using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Blazor.Diagrams.Core.Models
{
    public class PortModel : Model
    {
        private readonly List<LinkModel> _links = new List<LinkModel>(4);

        public PortModel(NodeModel parent, PortAlignment alignment = PortAlignment.BOTTOM, Point? position = null, Size? size = null)
        {
            Parent = parent;
            Alignment = alignment;
            Position = position ?? Point.Zero;
            Size = size ?? Size.Zero;
        }

        public PortModel(string id, NodeModel parent, PortAlignment alignment = PortAlignment.BOTTOM, Point? position = null,
            Size? size = null) : base(id)
        {
            Parent = parent;
            Alignment = alignment;
            Position = position ?? Point.Zero;
            Size = size ?? Size.Zero;
        }

        public NodeModel Parent { get; }
        public PortAlignment Alignment { get; }
        public Point Offset { get; set; } = Point.Zero;
        public Point Position { get; set; }
        public Size Size { get; set; }
        public ReadOnlyCollection<LinkModel> Links => _links.AsReadOnly();

        public void RefreshAll()
        {
            Refresh();
            _links.ForEach(l => l.Refresh());
        }

        internal void AddLink(LinkModel link) => _links.Add(link);

        internal void RemoveLink(LinkModel link) => _links.Remove(link);
    }
}
