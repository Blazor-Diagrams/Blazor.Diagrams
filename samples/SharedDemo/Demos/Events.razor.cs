using Blazor.Diagrams.Core;
using Blazor.Diagrams.Core.Models;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace SharedDemo.Demos
{
    public class EventsComponent : ComponentBase
    {
        protected readonly DiagramManager diagramManager = new DiagramManager();
        protected readonly List<string> events = new List<string>();

        protected override void OnInitialized()
        {
            base.OnInitialized();

            RegisterEvents();

            var node1 = NewNode(50, 50);
            var node2 = NewNode(300, 300);
            diagramManager.AddNode(node1);
            diagramManager.AddNode(node2);
            diagramManager.AddNode(NewNode(300, 50));

            diagramManager.AddLink(node1.GetPort(PortAlignment.Right), node2.GetPort(PortAlignment.Left));
        }

        private void RegisterEvents()
        {
            diagramManager.Changed += () =>
            {
                events.Add("Changed");
                StateHasChanged();
            };

            diagramManager.NodeAdded += (n) => events.Add($"NodeAdded, NodeId={n.Id}");
            diagramManager.NodeRemoved += (n) => events.Add($"NodeRemoved, NodeId={n.Id}");

            diagramManager.SelectionChanged += (m, s) =>
            {
                events.Add($"SelectionChanged, Id={m.Id}, Type={m.GetType().Name}, Selected={s}");
                StateHasChanged();
            };

            diagramManager.LinkAdded += (l) => events.Add($"LinkAdded, LinkId={l.Id}");

            diagramManager.LinkAttached += (l) =>
            {
                events.Add($"LinkAttached, LinkId={l.Id}");
                StateHasChanged();
            };

            diagramManager.LinkRemoved += (l) => events.Add($"LinkRemoved, LinkId={l.Id}");

            diagramManager.MouseUp += (m, e) => {
                events.Add($"NodeMoved, NodeId={m.Id}");
                StateHasChanged();
            };

        }

        private NodeModel NewNode(double x, double y)
        {
            var node = new NodeModel(new Point(x, y));
            node.AddPort(PortAlignment.Bottom);
            node.AddPort(PortAlignment.Top);
            node.AddPort(PortAlignment.Left);
            node.AddPort(PortAlignment.Right);
            return node;
        }
    }
}
