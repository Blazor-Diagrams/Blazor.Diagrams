using Blazor.Diagrams.Core.Models.Base;
using Blazor.Diagrams.Core.Models.Core;

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

        public LinkType Type { get; set; }
        public PortModel SourcePort { get; private set; }
        public PortModel? TargetPort { get; private set; }
        public bool IsAttached => TargetPort != null;
        public Point? OnGoingPosition { get; set; }

        public void SetSourcePort(PortModel port)
        {
            if (port != SourcePort)
            {
                SourcePort.RemoveLink(this);
                port.AddLink(this);
                SourcePort = port;
            }
        }
        
        public void SetTargetPort(PortModel port)
        {
            if (port != TargetPort)
            {
                TargetPort?.RemoveLink(this);
                port.AddLink(this);
                TargetPort = port;
            }
        }
    }
}
