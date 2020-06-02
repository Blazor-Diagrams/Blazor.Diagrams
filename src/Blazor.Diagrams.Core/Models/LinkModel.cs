namespace Blazor.Diagrams.Core.Models
{
    public class LinkModel : Model
    {
        public LinkModel(PortModel sourcePort, PortModel targetPort)
        {
            SourcePort = sourcePort;
            TargetPort = targetPort;
        }

        public LinkModel(string id, PortModel sourcePort, PortModel targetPort) : base(id)
        {
            SourcePort = sourcePort;
            TargetPort = targetPort;
        }

        public PortModel SourcePort { get; }
        public PortModel TargetPort { get; }
    }
}
