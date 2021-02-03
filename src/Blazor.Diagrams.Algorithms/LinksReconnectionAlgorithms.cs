using Blazor.Diagrams.Algorithms.Extensions;
using Blazor.Diagrams.Core;
using Blazor.Diagrams.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace Blazor.Diagrams.Algorithms
{
    public static class LinksReconnectionAlgorithms
    {
        public static void ReconnectLinksToClosestPorts(this DiagramManager diagramManager)
        {
            // Only refresh ports once
            var portsToRefresh = new HashSet<PortModel>();

            foreach (var link in diagramManager.Links.ToArray())
            {
                if (link.TargetPort == null)
                    continue;

                var sourcePorts = link.SourcePort.Parent.Ports;
                var targetPorts = link.TargetPort.Parent.Ports;

                // Find the ports with minimal distance
                var minDistance = double.MaxValue;
                var minSourcePort = link.SourcePort;
                var minTargetPort = link.TargetPort;
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
                if (link.SourcePort != minSourcePort)
                {
                    portsToRefresh.Add(link.SourcePort);
                    portsToRefresh.Add(minSourcePort);
                    link.SetSourcePort(minSourcePort);
                }

                if (link.TargetPort != minTargetPort)
                {
                    portsToRefresh.Add(link.TargetPort);
                    portsToRefresh.Add(minTargetPort);
                    link.SetTargetPort(minTargetPort);
                }
            }

            foreach (var port in portsToRefresh)
                port.Refresh();
        }
    }
}
