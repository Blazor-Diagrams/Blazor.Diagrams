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
        protected readonly Diagram diagram = new Diagram();

        protected override void OnInitialized()
        {
            base.OnInitialized();

            diagram.Options.Groups.Enabled = true;
            diagram.Nodes.Add(new NodeModel(new Point(300, 50)));
            diagram.Nodes.Add(new NodeModel(new Point(300, 400)));
        }

        protected void AddNode()
        {
            var x = _random.Next(0, (int)diagram.Container.Width - 120);
            var y = _random.Next(0, (int)diagram.Container.Height - 100);
            diagram.Nodes.Add(new NodeModel(new Point(x, y)));
        }

        protected void RemoveNode()
        {
            var i = _random.Next(0, diagram.Nodes.Count);
            diagram.Nodes.Remove(diagram.Nodes[i]);
        }

        protected void AddPort()
        {
            var node = diagram.Nodes.FirstOrDefault(n => n.Selected);
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
            var node = diagram.Nodes.FirstOrDefault(n => n.Selected);
            if (node == null)
                return;

            if (node.Ports.Count == 0)
                return;

            var i = _random.Next(0, node.Ports.Count);
            var port = node.Ports[i];

            diagram.Links.Remove(port.Links.ToArray());
            node.RemovePort(port);
            node.Refresh();
        }

        protected void AddLink()
        {
            var selectedNodes = diagram.Nodes.Where(n => n.Selected).ToArray();
            if (selectedNodes.Length != 2)
                return;

            var node1 = selectedNodes[0];
            var node2 = selectedNodes[1];

            if (node1 == null || node1.Ports.Count == 0 || node2 == null || node2.Ports.Count == 0)
                return;

            var sourcePort = node1.Ports[_random.Next(0, node1.Ports.Count)];
            var targetPort = node2.Ports[_random.Next(0, node2.Ports.Count)];
            diagram.Links.Add(new LinkModel(sourcePort, targetPort));
        }
    }
}
