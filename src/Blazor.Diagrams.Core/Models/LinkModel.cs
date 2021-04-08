using Blazor.Diagrams.Core.Models.Base;

namespace Blazor.Diagrams.Core.Models
{
    public class LinkModel : BaseLinkModel
    {
        public LinkModel(PortModel sourcePort, PortModel? targetPort = null) : base(sourcePort, targetPort) { }

        public LinkModel(NodeModel sourceNode, NodeModel? targetNode) : base(sourceNode, targetNode) { }

        public LinkModel(string id, PortModel sourcePort, PortModel? targetPort = null) : base(id, sourcePort, targetPort) { }

        public LinkModel(string id, NodeModel sourceNode, NodeModel? targetNode) : base(id, sourceNode, targetNode) { }

        public string? Color { get; set; }
        public string? SelectedColor { get; set; }
        public double Width { get; set; } = 2;
    }
}
