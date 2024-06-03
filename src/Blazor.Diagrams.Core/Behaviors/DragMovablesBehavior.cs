using Blazor.Diagrams.Core.Behaviors.Base;
using Blazor.Diagrams.Core.Events;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;
using DiagramPoint = Blazor.Diagrams.Core.Geometry.Point;

namespace Blazor.Diagrams.Core.Behaviors;

public class DragMovablesBehavior : Behavior
{
    record NodeMoveablePositions(Point position)
    {
        public Dictionary<NodeModel, DiagramPoint> ChildPositions { get; } = new();
    }

    private readonly Dictionary<NodeModel, NodeMoveablePositions> _initialPositions;
    private double? _lastClientX;
    private double? _lastClientY;
    private bool _moved;
    double _totalMovedX = 0;
    double _totalMovedY = 0;

    public const double CHILD_NODE_MIN_OFFSET_TOP = 40;
    public const double CHILD_NODE_MIN_OFFSET_BOTTOM = 5;

    public DragMovablesBehavior(Diagram diagram) : base(diagram)
    {
        _initialPositions = new Dictionary<NodeModel, NodeMoveablePositions>();
        Diagram.PointerDown += OnPointerDown;
        Diagram.PointerMove += OnPointerMove;
        Diagram.PointerUp += OnPointerUp;
        Diagram.PanChanged += OnPanChanged;
    }

    private void OnPointerDown(Model? model, PointerEventArgs e)
    {
        Console.WriteLine("On Pointer Down...");
        if (model is not NodeModel)
            return;

        _initialPositions.Clear();
        foreach (var sm in Diagram.GetSelectedModels())
        {
            if (sm is not NodeModel node || node.Locked)
            {
                continue;
            }

            // Special case: groups without auto size on
            if (node.Group != null && !node.Group.AutoSize)
            {
                continue;
            }

            var position = node.Position;

            if (Diagram.Options.GridSnapToCenter && node is NodeModel nodeModel)
            {
                position = new Point(node.Position.X + (nodeModel?.Size?.Width ?? 0) / 2,
                                node.Position.Y + (nodeModel?.Size?.Height ?? 0) / 2);
            }
            _initialPositions.Add(node, new NodeMoveablePositions(position));
        }

        _lastClientX = e.ClientX;
        _lastClientY = e.ClientY;
        _moved = false;
    }

    private void OnPointerMove(Model? model, PointerEventArgs e)
    {
        if (!_moved)
        {
            foreach (var node in _initialPositions.Keys.ToArray())
            {
                if (node is NodeModel nodeModel)
                {
                    var parent = nodeModel.GetParentNode();
                    while (parent != null)
                    {
                        if (_initialPositions.ContainsKey(parent))
                        {
                            _initialPositions.Remove(nodeModel);
                            break;
                        }

                        parent = parent.GetParentNode();
                    }
                }
            }

            foreach (var (node, positions) in _initialPositions)
            {
                if (node is NodeModel nodeModel)
                {
                    foreach (var child in nodeModel.GetAllChildNodes())
                    {
                        if (!child.Selected)
                        {
                            Diagram.SelectModel(child, false);
                        }

                        positions.ChildPositions[child] = child.Position;
                    }
                }
            }
        }
        if (_initialPositions.Count == 0 || _lastClientX == null || _lastClientY == null)
        {
            return;
        }

        _moved = true;

        var dx = (e.ClientX - _lastClientX.Value) / Diagram.Zoom;
        var dy = (e.ClientY - _lastClientY.Value) / Diagram.Zoom;

        _totalMovedX += dx;
        _totalMovedY += dy;

        MoveNodes(model, _totalMovedX, _totalMovedY);

        _lastClientX = e.ClientX;
        _lastClientY = e.ClientY;
    }

    public void OnPanChanged(double deltaX, double deltaY)
    {
        if (_initialPositions.Count == 0 || _lastClientX == null || _lastClientY == null)
            return;

        _moved = true;

        _totalMovedX += deltaX;
        _totalMovedY += deltaY;

        MoveNodesOnPan(null, _totalMovedX, _totalMovedY);
    }

    private void MoveNodesOnPan(Model? model, double deltaX, double deltaY)
    {
        foreach (var (movable, initialPosition) in _initialPositions)
        {
            var ndx = ApplyGridSize(deltaX + initialPosition.position.X);
            var ndy = ApplyGridSize(deltaY + initialPosition.position.Y);
            if (Diagram.Options.GridSnapToCenter && movable is NodeModel node)
            {
                node.SetPosition(ndx - (node.Size?.Width ?? 0) / 2, ndy - (node.Size?.Height ?? 0) / 2);
            }
            else
            {
                movable.SetPosition(ndx, ndy);
            }
        }
    }

    private double ApplyGridSize(double n)
    {
        if (Diagram.Options.GridSize == null)
            return n;

        var gridSize = Diagram.Options.GridSize.Value;
        return gridSize * Math.Floor((n + gridSize / 2.0) / gridSize);
    }

    void MoveNodes(Model? model, double deltaX, double deltaY)
    {
        foreach (var (node, positions) in _initialPositions)
        {
            if (Diagram.Options.GridSnapToCenter && node is NodeModel nodeModel)
            {
                var ndx = ApplyGridSize(deltaX + positions.position.X);
                var ndy = ApplyGridSize(deltaY - positions.position.Y);

                node.SetPosition(ndx - (node.Size?.Width ?? 0) / 2, ndy - (node.Size?.Height ?? 0) / 2);
            }
            else
            {
                SetNodePosition(node, positions.position.X + deltaX, positions.position.Y + deltaY);
                deltaX = node.Position.X - positions.position.X;
                deltaY = node.Position.Y - positions.position.Y;

                foreach (var (childNode, childPosition) in positions.ChildPositions)
                {
                    SetNodePosition(childNode, childPosition.X + deltaX, childPosition.Y + deltaY);
                }

                if (node is NodeModel node1Model)
                {
                    node1Model.TriggerMoving();
                }
            }
        }
    }

    void SetNodePosition(NodeModel node, double x, double y)
    {
        if (node is NodeModel nodeModel && nodeModel.GetParentNode() != null)
        {
            x = Clamp(x, nodeModel.Size?.Width, nodeModel.GetParentNode().Position.X, nodeModel.GetParentNode().Size?.Width);
            var parentY = nodeModel.GetParentNode().Position.Y + CHILD_NODE_MIN_OFFSET_TOP;
            var parentH = nodeModel.GetParentNode().Size?.Height - CHILD_NODE_MIN_OFFSET_TOP - CHILD_NODE_MIN_OFFSET_BOTTOM;
            y = Clamp(y, nodeModel.Size?.Height, parentY, parentH);

        }

        node.SetPosition(x, y);
    }

    double Clamp(double position, double? size, double? parentPosition, double? parentSize)
    {
        var clamped = position;

        if (size != null && parentPosition != null && parentSize != null)
        {
            if (position < parentPosition)
            {
                clamped = (double)parentPosition;
            }
            else if (position + size > parentPosition + parentSize)
            {
                clamped = (double)(parentPosition + parentSize - size);
            }
        }

        return clamped;
    }

    private void OnPointerUp(Model? model, PointerEventArgs e)
    {
        if (_initialPositions.Count == 0)
            return;

        if (_moved)
        {
            foreach (var (movable, childMovableNodes) in _initialPositions)
            {
                movable.TriggerMoved();
                if (movable is NodeModel movableNode)
                {
                    foreach (var child in movableNode.GetAllChildNodes())
                    {
                        if (childMovableNodes.ChildPositions.ContainsKey(child))
                        {
                            child.TriggerMoved();
                        }
                    }
                }
            }
        }

        _initialPositions.Clear();
        _totalMovedX = 0;
        _totalMovedY = 0;
        _lastClientX = null;
        _lastClientY = null;
    }

    public override void Dispose()
    {
        _initialPositions.Clear();

        Diagram.PointerDown -= OnPointerDown;
        Diagram.PointerMove -= OnPointerMove;
        Diagram.PointerUp -= OnPointerUp;
        Diagram.PanChanged -= OnPanChanged;
    }
}