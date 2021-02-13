using Blazor.Diagrams.Core.Models.Core;
using System;

namespace Blazor.Diagrams.Core.Models.Base
{
    public abstract class BaseLinkModel : SelectableModel
    {
        public event Action? SourcePortChanged;
        public event Action? TargetPortChanged;

        public BaseLinkModel(PortModel sourcePort, PortModel? targetPort = null)
        {
            SourcePort = sourcePort;
            TargetPort = targetPort;
        }

        public BaseLinkModel(string id, PortModel sourcePort, PortModel? targetPort = null) : base(id)
        {
            SourcePort = sourcePort;
            TargetPort = targetPort;
        }

        public PortModel SourcePort { get; private set; }
        public PortModel? TargetPort { get; private set; }
        public bool IsAttached => TargetPort != null;
        public Point? OnGoingPosition { get; set; }
        public Router? Router { get; set; }
        public PathGenerator? PathGenerator { get; set; }
        public LinkMarker? SourceMarker { get; set; }
        public LinkMarker? TargetMarker { get; set; }

        public void SetSourcePort(PortModel port)
        {
            if (SourcePort == port)
                return;

            SourcePort.RemoveLink(this);
            SourcePort = port;
            SourcePort.AddLink(this);
            SourcePortChanged?.Invoke();
        }

        public void SetTargetPort(PortModel port)
        {
            if (TargetPort == port)
                return;

            TargetPort?.RemoveLink(this);
            TargetPort = port;
            TargetPort.AddLink(this);
            TargetPortChanged?.Invoke();
        }
    }
}
