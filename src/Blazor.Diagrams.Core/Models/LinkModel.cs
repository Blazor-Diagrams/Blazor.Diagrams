using Blazor.Diagrams.Core.Models.Base;

namespace Blazor.Diagrams.Core.Models
{
    public class LinkModel : SelectableModel
    {
        public LinkModel(PortModel sourcePort, PortModel? targetPort = null)
        {
            SourcePort = sourcePort;
            TargetPort = targetPort;
        }

        public LinkModel(string id, PortModel sourcePort, PortModel? targetPort = null) : base(id)
        {
            SourcePort = sourcePort;
            TargetPort = targetPort;
        }

        public LinkType Type { get; set; } = LinkType.Curved;
        public PortModel SourcePort { get; }
        public PortModel? TargetPort { get; private set; }
        public bool IsAttached => TargetPort != null;
        public Point? OnGoingPosition { get; set; }

        public void SetTargetPort(PortModel port)
        {
            if (IsAttached)
                return;

            TargetPort = port;
        }
    }

    public enum LinkType
    {
        Line,
        Curved
    }
}
