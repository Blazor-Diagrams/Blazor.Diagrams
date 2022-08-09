using Blazor.Diagrams.Core.Anchors;
using Blazor.Diagrams.Core.Models.Base;

namespace Blazor.Diagrams.Core.Models
{
    public class LinkModel : BaseLinkModel
    {
        public LinkModel(Anchor source, Anchor? target = null) : base(source, target) { }

        public LinkModel(string id, Anchor source, Anchor? target = null) : base(id, source, target) { }

        public LinkModel(PortModel sourcePort, PortModel? targetPort = null)
            : base(new SinglePortAnchor(sourcePort), targetPort is null ? null : new SinglePortAnchor(targetPort)) { }

        public LinkModel(NodeModel sourceNode, NodeModel? targetNode)
            : base(new ShapeIntersectionAnchor(sourceNode), targetNode is null ? null : new ShapeIntersectionAnchor(targetNode)) { }

        public LinkModel(string id, PortModel sourcePort, PortModel? targetPort = null)
            : base(id, new SinglePortAnchor(sourcePort), targetPort is null ? null : new SinglePortAnchor(targetPort)) { }

        public LinkModel(string id, NodeModel sourceNode, NodeModel? targetNode)
            : base(id, new ShapeIntersectionAnchor(sourceNode), targetNode is null ? null : new ShapeIntersectionAnchor(targetNode)) { }

        public string? Color { get; set; }
        public string? SelectedColor { get; set; }
        public double Width { get; set; } = 2;
    }
}
