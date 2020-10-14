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

            var node1 = new NodeModel(new Point(300, 50));
            var node2 = new NodeModel(new Point(300, 400));
            diagramManager.AddNodes(node1, node2);
        }

        protected void AddNode()
        {
            var x = _random.Next(0, (int)diagramManager.Container.Width - 120);
            var y = _random.Next(0, (int)diagramManager.Container.Height - 100);
            diagramManager.AddNode(new NodeModel(new Point(x, y)));
        }

        protected void RemoveNode()
        {
            var i = _random.Next(0, diagramManager.Nodes.Count);
            diagramManager.RemoveNode(diagramManager.Nodes.ElementAt(i));
        }

        protected void AddPort()
        {
            var model = diagramManager.SelectedModels.FirstOrDefault(sm => sm is NodeModel);
            if (model == null)
                return;

            var node = model as NodeModel;
            if (node.GetPort(PortAlignment.Top) == null)
            {
                node.AddPort(PortAlignment.Top);
                node.Refresh();
            }
            else if (node.GetPort(PortAlignment.Right) == null)
            {
                node.AddPort(PortAlignment.Right);
                node.Refresh();
            }
            else if (node.GetPort(PortAlignment.Bottom) == null)
            {
                node.AddPort(PortAlignment.Bottom);
                node.Refresh();
            }
            else if (node.GetPort(PortAlignment.Left) == null)
            {
                node.AddPort(PortAlignment.Left);
                node.Refresh();
            }
        }

        protected void RemovePort()
        {
            var model = diagramManager.SelectedModels.FirstOrDefault(sm => sm is NodeModel);
            if (model == null)
                return;

            var node = model as NodeModel;
            if (node.Ports.Count == 0)
                return;

            var i = _random.Next(0, node.Ports.Count);
            var port = node.Ports[i];

            foreach (var link in port.Links)
                diagramManager.RemoveLink(link);

            node.RemovePort(port);
            node.Refresh();
        }

        protected void AddLink()
        {
            if (diagramManager.SelectedModels.Count != 2)
                return;

            var node1 = diagramManager.SelectedModels.ElementAt(0) as NodeModel;
            var node2 = diagramManager.SelectedModels.ElementAt(1) as NodeModel;

            if (node1 == null || node1.Ports.Count == 0 || node2 == null || node2.Ports.Count == 0)
                return;

            var sourcePort = node1.Ports[_random.Next(0, node1.Ports.Count)];
            var targetPort = node2.Ports[_random.Next(0, node2.Ports.Count)];
            diagramManager.AddLink(sourcePort, targetPort);
        }
    }
}
