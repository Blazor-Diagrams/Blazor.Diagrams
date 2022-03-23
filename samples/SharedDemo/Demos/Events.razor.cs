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

        protected bool MouseEvents { get; set; } = true;
        protected bool TouchEvents { get; set; } = true;
        protected bool DiagramEvents { get; set; } = true;
        protected bool NodeEvents { get; set; } = true;
        protected bool LinkEvents { get; set; } = true;
        protected bool SelectionEvents { get; set; } = true;

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
                if (DiagramEvents)
                    events.Add("Changed");
                StateHasChanged();
            };

            diagram.Nodes.Added += (n) =>
            {
                if (NodeEvents)
                    events.Add($"NodesAdded, NodeId={n.Id}");
            };
            diagram.Nodes.Removed += (n) =>
            {
                if (NodeEvents)
                    events.Add($"NodesRemoved, NodeId={n.Id}");
            };

            diagram.SelectionChanged += (m) =>
            {
                if (SelectionEvents)
                    events.Add($"SelectionChanged, Id={m.Id}, Type={m.GetType().Name}, Selected={m.Selected}");
                StateHasChanged();
            };

            diagram.Links.Added += (l) =>
            {
                l.NodesLinked += (model, sourcePort, targetPort) =>
                {
                    if (NodeEvents)
                        events.Add($"Two Nodes Linked, {sourcePort.Id} and {targetPort.Id}");
                };
                if (LinkEvents)
                    events.Add($"Links.Added, LinkId={l.Id}");
            };

            diagram.Links.Removed += (l) =>
            {
                if (LinkEvents)
                    events.Add($"Links.Removed, LinkId={l.Id}");
            };

            diagram.MouseDown += (m, e) =>
            {
                if (MouseEvents)
                    events.Add($"MouseDown, Type={m?.GetType().Name}, ModelId={m?.Id}");
                StateHasChanged();
            };

            diagram.MouseUp += (m, e) =>
            {
                if (MouseEvents)
                    events.Add($"MouseUp, Type={m?.GetType().Name}, ModelId={m?.Id}");
                StateHasChanged();
            };

            diagram.TouchStart += (m, e) =>
            {
                if (TouchEvents)
                    events.Add($"TouchStart, Type={m?.GetType().Name}, ModelId={m?.Id}");
                StateHasChanged();
            };

            diagram.TouchEnd += (m, e) =>
            {
                if (TouchEvents)
                    events.Add($"TouchEnd, Type={m?.GetType().Name}, ModelId={m?.Id}");
                StateHasChanged();
            };

            diagram.MouseClick += (m, e) =>
            {
                if (MouseEvents)
                    events.Add($"MouseClick, Type={m?.GetType().Name}, ModelId={m?.Id}");
                StateHasChanged();
            };

            diagram.MouseDoubleClick += (m, e) =>
            {
                if (MouseEvents)
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
