using Blazor.Diagrams.Core.Geometry;
using System;
using System.Collections.Generic;

namespace Blazor.Diagrams.Core.Models.Base
{
    public abstract class BaseLinkModel : SelectableModel
    {
        /// <summary>
        /// An event that fires when the SourcePort changes.
        /// <para>The PortModel instance in the parameters is the old value.</para>
        /// </summary>
        public event Action<PortModel?>? SourcePortChanged;
        /// <summary>
        /// An event that fires when the TargetPort changes.
        /// <para>The PortModel instance in the parameters is the old value.</para>
        /// </summary>
        public event Action<PortModel?>? TargetPortChanged;

        public BaseLinkModel(NodeModel sourceNode, NodeModel? targetNode)
        {
            SourceNode = sourceNode;
            TargetNode = targetNode;
        }

        public BaseLinkModel(string id, NodeModel sourceNode, NodeModel? targetNode) : base(id)
        {
            SourceNode = sourceNode;
            TargetNode = targetNode;
        }

        public BaseLinkModel(PortModel sourcePort, PortModel? targetPort = null)
        {
            SourcePort = sourcePort;
            TargetPort = targetPort;
            SourceNode = SourcePort.Parent;
            TargetNode = targetPort?.Parent;
        }

        public BaseLinkModel(string id, PortModel sourcePort, PortModel? targetPort = null) : base(id)
        {
            SourcePort = sourcePort;
            TargetPort = targetPort;
            SourceNode = SourcePort.Parent;
            TargetNode = targetPort?.Parent;
        }

        public NodeModel SourceNode { get; }
        public NodeModel? TargetNode { get; }
        public PortModel? SourcePort { get; private set; }
        public PortModel? TargetPort { get; private set; }
        public bool IsAttached => TargetNode != null || TargetPort != null;
        public bool IsPortless => SourcePort == null;
        public Point? OnGoingPosition { get; set; }
        public Router? Router { get; set; }
        public PathGenerator? PathGenerator { get; set; }
        public LinkMarker? SourceMarker { get; set; }
        public LinkMarker? TargetMarker { get; set; }
        public bool Segmentable { get; set; } = false;
        public List<LinkVertexModel> Vertices { get; } = new List<LinkVertexModel>();
        public List<LinkLabelModel> Labels { get; set; } = new List<LinkLabelModel>();

        public void SetSourcePort(PortModel port)
        {
            if (SourcePort == port)
                return;

            var old = SourcePort;
            SourcePort?.RemoveLink(this);
            SourcePort = port;
            SourcePort.AddLink(this);
            SourcePortChanged?.Invoke(old);
        }

        public void SetTargetPort(PortModel? port)
        {
            if (TargetPort == port)
                return;

            var old = TargetPort;
            TargetPort?.RemoveLink(this);
            TargetPort = port;
            TargetPort?.AddLink(this);
            TargetPortChanged?.Invoke(old);
        }
    }
}
