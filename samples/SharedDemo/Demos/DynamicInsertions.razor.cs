using Blazor.Diagrams.Core;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Core;
using Microsoft.AspNetCore.Components;
using System;
using System.Linq;

namespace SharedDemo.Demos
{
    public class DynamicInsertionsComponent : ComponentBase
    {
        private static readonly Random _random = new Random();
        protected readonly DiagramManager diagramManager = new DiagramManager();

        protected override void OnInitialized()
        {
            base.OnInitialized();

            diagramManager.Options.Groups.Enabled = true;
            var node1 = new NodeModel(new Point(300, 50));
            var node2 = new NodeModel(new Point(300, 400));
            diagramManager.Nodes.Add(node1, node2);
        }

        protected void AddNode()
        {
            var x = _random.Next(0, (int)diagramManager.Container.Width - 120);
            var y = _random.Next(0, (int)diagramManager.Container.Height - 100);
            diagramManager.Nodes.Add(new NodeModel(new Point(x, y)));
        }

        protected void RemoveNode()
        {
            var i = _random.Next(0, diagramManager.Nodes.Count);
            diagramManager.Nodes.Remove(diagramManager.Nodes[i]);
        }

        protected void AddPort()
        {
            var node = diagramManager.Nodes.FirstOrDefault(n => n.Selected);
            if (node == null)
                return;

            foreach(PortAlignment portAlignment in Enum.GetValues(typeof(PortAlignment)))
            {
                if(node.GetPort(portAlignment) == null)
                {
                    node.AddPort(portAlignment);
                    node.Refresh();
                    break;
                }
            }            
        }

        protected void RemovePort()
        {
            var node = diagramManager.Nodes.FirstOrDefault(n => n.Selected);
            if (node == null)
                return;

            if (node.Ports.Count == 0)
                return;

            var i = _random.Next(0, node.Ports.Count);
            var port = node.Ports[i];

            diagramManager.Links.Remove(port.Links.ToArray());
            node.RemovePort(port);
            node.Refresh();
        }

        protected void AddLink()
        {
            var selectedNodes = diagramManager.Nodes.Where(n => n.Selected).ToArray();
            if (selectedNodes.Length != 2)
                return;

            var node1 = selectedNodes[0];
            var node2 = selectedNodes[1];

            if (node1 == null || node1.Ports.Count == 0 || node2 == null || node2.Ports.Count == 0)
                return;

            var sourcePort = node1.Ports[_random.Next(0, node1.Ports.Count)];
            var targetPort = node2.Ports[_random.Next(0, node2.Ports.Count)];
            diagramManager.Links.Add(new LinkModel(sourcePort, targetPort));
        }
    }
}
