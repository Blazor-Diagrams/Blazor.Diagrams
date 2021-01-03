using Blazor.Diagrams.Core.Models.Base;
using Blazor.Diagrams.Core.Models.Core;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Blazor.Diagrams.Core.Models
{
    public class PortModel : Model
    {
        private readonly List<LinkModel> _links = new List<LinkModel>(4);

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
        public Size Size { get; set; }
        public ReadOnlyCollection<LinkModel> Links => _links.AsReadOnly();
        public bool Initialized { get; internal set; }

        public void RefreshAll()
        {
            Refresh();
            RefreshLinks();
        }

        public void RefreshLinks() => _links.ForEach(l => l.Refresh());

        public T GetParent<T>() where T : NodeModel => (T)Parent;

        public virtual bool CanAttachTo(PortModel port) => port != this && !port.Locked && Parent != port.Parent;

        internal void AddLink(LinkModel link) => _links.Add(link);

        internal void RemoveLink(LinkModel link) => _links.Remove(link);
    }
}
