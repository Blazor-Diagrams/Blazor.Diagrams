namespace Blazor.Diagrams.Core.Models
{
    public class Link : Model
    {
        public Link(Port sourcePort, Port targetPort)
        {
            SourcePort = sourcePort;
            TargetPort = targetPort;
        }

        public Link(string id, Port sourcePort, Port targetPort) : base(id)
        {
            SourcePort = sourcePort;
            TargetPort = targetPort;
        }

        public Port SourcePort { get; }
        public Port TargetPort { get; }
    }
}
