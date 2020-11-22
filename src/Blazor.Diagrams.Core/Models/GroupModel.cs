using Blazor.Diagrams.Core.Models.Base;
using Blazor.Diagrams.Core.Models.Core;
using System;
using System.Linq;

namespace Blazor.Diagrams.Core.Models
{
    public class GroupModel : MovableModel, IDisposable
    {
        private readonly DiagramManager _diagramManager;

        public GroupModel(DiagramManager diagramManager, NodeModel[] nodes)
        {
            _diagramManager = diagramManager;
            Nodes = nodes;
            Initialize();
        }

        public NodeModel[] Nodes { get; private set; }
        public Size Size { get; private set; } = Size.Zero;
        public Point? TopLeftBoundary { get; private set; }
        public Point MovedBy => TopLeftBoundary == null ? Point.Zero : (Position - TopLeftBoundary);

        public void Dispose()
        {
            foreach (var node in Nodes)
                node.Moving -= Node_Moving;
        }

        private void Initialize()
        {
            foreach (var node in Nodes)
            {
                node.Group = this;
                node.Moving += Node_Moving;
            }

            UpdateDimensions();
        }

        private void Node_Moving(NodeModel node)
        {
            // Todo: optimize
            UpdateDimensions();

            // Refresh the position of the other nodes in the group
            foreach (var gnode in Nodes)
            {
                if (gnode == node)
                    continue;

                gnode.Refresh();
            }
        }

        private void UpdateDimensions()
        {
            (var nodesMinX, var nodesMaxX, var nodesMinY, var nodesMaxY) = _diagramManager.GetNodesRect(Nodes.ToList());
            Size = new Size(nodesMaxX - nodesMinX, nodesMaxY - nodesMinY);

            var diff = TopLeftBoundary == null ? Point.Zero : (Position - TopLeftBoundary);
            TopLeftBoundary = new Point(nodesMinX, nodesMinY);
            Position = new Point(nodesMinX + diff.X, nodesMinY + diff.Y);
            Refresh();
        }

        public override void SetPosition(double x, double y)
        {
            var diff = Position.Substract(x, y);
            Console.WriteLine(diff);
            base.SetPosition(x, y);

            foreach (var port in Nodes.SelectMany(n => n.Ports))
            {
                var old = port.Position;
                port.Position -= diff;
                Console.WriteLine($"Old={old}, New={port.Position}");
                port.RefreshAll();
            }

            Refresh();

            //foreach (var link in Nodes.SelectMany(n => n.Ports.SelectMany(p => p.Links)).Distinct())
            //    link.Refresh();
        }
    }
}
