using Blazor.Diagrams.Core;
using Blazor.Diagrams.Core.Anchors;
using Blazor.Diagrams.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace Blazor.Diagrams.Algorithms
{
    public static class LinksReconnectionAlgorithms
    {
        public static void ReconnectLinksToClosestPorts(this DiagramBase diagram)
        {
            // Only refresh ports once
            var portsToRefresh = new HashSet<PortModel>();

            foreach (var link in diagram.Links.ToArray())
            {
                if (link.Target == null)
                    continue;

                if (link.Source is not SinglePortAnchor spa1 || link.Target is not SinglePortAnchor spa2)
                    continue;

                var sourcePorts = spa1.Node.Ports;
                var targetPorts = spa2.Node.Ports;

                // Find the ports with minimal distance
                var minDistance = double.MaxValue;
                var minSourcePort = spa1.Port;
                var minTargetPort = spa2.Port;
                foreach (var sourcePort in sourcePorts)
                {
                    foreach (var targetPort in targetPorts)
                    {
                        var distance = sourcePort.Position.DistanceTo(targetPort.Position);
                        if (distance < minDistance)
                        {
                            minDistance = distance;
                            minSourcePort = sourcePort;
                            minTargetPort = targetPort;
                        }
                    }
                }

                // Reconnect
                if (spa1.Port != minSourcePort)
                {
                    portsToRefresh.Add(spa1.Port);
                    portsToRefresh.Add(minSourcePort);
                    link.SetSource(new SinglePortAnchor(minSourcePort));
                }

                if (spa2.Port != minTargetPort)
                {
                    portsToRefresh.Add(spa2.Port);
                    portsToRefresh.Add(minTargetPort);
                    link.SetTarget(new SinglePortAnchor(minTargetPort));
                }
            }

            foreach (var port in portsToRefresh)
                port.Refresh();
        }
    }
}
