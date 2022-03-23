using Blazor.Diagrams.Core;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace SharedDemo.Demos
{
    public class EventsComponent : ComponentBase
    {
        protected readonly Diagram diagram = new Diagram();
        protected readonly List<string> events = new List<string>();

        protected override void OnInitialized()
        {
            base.OnInitialized();

            RegisterEvents();

            var node1 = NewNode(50, 50);
            var node2 = NewNode(300, 300);
            diagram.Nodes.Add(new[] { node1, node2, NewNode(300, 50) });
            diagram.Links.Add(new LinkModel(node1.GetPort(PortAlignment.Right), node2.GetPort(PortAlignment.Left)));
        }

        private void RegisterEvents()
        {
            diagram.Changed += () =>
            {
                events.Add("Changed");
                StateHasChanged();
            };

            diagram.Nodes.Added += (n) => events.Add($"NodesAdded, NodeId={n.Id}");
            diagram.Nodes.Removed += (n) => events.Add($"NodesRemoved, NodeId={n.Id}");

            diagram.SelectionChanged += (m) =>
            {
                events.Add($"SelectionChanged, Id={m.Id}, Type={m.GetType().Name}, Selected={m.Selected}");
                StateHasChanged();
            };

            diagram.Links.Added += (l) =>
            {
                l.NodesLinked += (model, sourcePort, targetPort) =>
                {
                    events.Add($"Two Nodes Linked, {sourcePort.Id} and {targetPort.Id}");
                };
                events.Add($"Links.Added, LinkId={l.Id}");
            };

            diagram.Links.Removed += (l) => events.Add($"Links.Removed, LinkId={l.Id}");

            diagram.MouseDown += (m, e) =>
            {
                events.Add($"MouseDown, Type={m?.GetType().Name}, ModelId={m?.Id}");
                StateHasChanged();
            };

            diagram.MouseUp += (m, e) =>
            {
                events.Add($"MouseUp, Type={m?.GetType().Name}, ModelId={m?.Id}");
                StateHasChanged();
            };

            diagram.TouchStart += (m, e) =>
            {
                events.Add($"TouchStart, Type={m?.GetType().Name}, ModelId={m?.Id}");
                StateHasChanged();
            };

            diagram.TouchEnd += (m, e) =>
            {
                events.Add($"TouchEnd, Type={m?.GetType().Name}, ModelId={m?.Id}");
                StateHasChanged();
            };

            diagram.MouseClick += (m, e) =>
            {
                events.Add($"MouseClick, Type={m?.GetType().Name}, ModelId={m?.Id}");
                StateHasChanged();
            };

            diagram.MouseDoubleClick += (m, e) =>
            {
                events.Add($"MouseDoubleClick, Type={m?.GetType().Name}, ModelId={m?.Id}");
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
