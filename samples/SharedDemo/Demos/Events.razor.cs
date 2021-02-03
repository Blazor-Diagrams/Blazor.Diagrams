using Blazor.Diagrams.Core;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Core;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;

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
            diagramManager.Nodes.Add(node1, node2, NewNode(300, 50));
            diagramManager.Links.Add(new LinkModel(node1.GetPort(PortAlignment.Right), node2.GetPort(PortAlignment.Left)));
        }

        private void RegisterEvents()
        {
            diagramManager.Changed += () =>
            {
                events.Add("Changed");
                StateHasChanged();
            };

            diagramManager.Nodes.Added += (nodes) => events.AddRange(nodes.Select(n => $"NodesAdded, NodeId={n.Id}"));
            diagramManager.Nodes.Removed += (nodes) => events.AddRange(nodes.Select(n => $"NodesRemoved, NodeId={n.Id}"));

            diagramManager.SelectionChanged += (m, s) =>
            {
                events.Add($"SelectionChanged, Id={m.Id}, Type={m.GetType().Name}, Selected={s}");
                StateHasChanged();
            };

            diagramManager.Links.Added += (links) => events.AddRange(links.Select(l => $"Links.Added, LinkId={l.Id}"));

            // Todo: replace with TargetPortChanged
            //diagramManager.LinkAttached += (l) =>
            //{
            //    events.Add($"LinkAttached, LinkId={l.Id}");
            //    StateHasChanged();
            //};

            diagramManager.Links.Removed += (links) => events.AddRange(links.Select(l => $"Links.Removed, LinkId={l.Id}"));

            diagramManager.MouseUp += (m, e) =>
            {
                events.Add($"MouseUp, Type={m?.GetType().Name}, ModelId={m?.Id}");
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
