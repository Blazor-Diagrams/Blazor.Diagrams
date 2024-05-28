using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models.Base;
using Blazor.Diagrams.Core.Events;
using System;
using System.Collections.Generic;
using Blazor.Diagrams.Core.Models;
using DiagramPoint = Blazor.Diagrams.Core.Geometry.Point;
using System.Linq;
using Blazor.Diagrams.Core.Behaviors.Base;

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
    }

    private void OnPointerDown(Model? model, PointerEventArgs e)
    {
        if (model is not NodeModel)
            return;

        _initialPositions.Clear();
        foreach (var sm in Diagram.GetSelectedModels())
        {
            if (sm is not NodeModel movable || movable.Locked)
            {
                continue;
            }

            _initialPositions.Add(movable, new NodeMoveablePositions(movable.Position));
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
                if (node is NodeModel jobNode)
                {
                    var parent = jobNode.ParentNode;
                    while (parent != null)
                    {
                        if (_initialPositions.ContainsKey(parent))
                        {
                            _initialPositions.Remove(jobNode);
                            break;
                        }

                        parent = parent.ParentNode;
                    }
                }
            }

            foreach (var (node, positions) in _initialPositions)
            {
                if (node is NodeModel jobNode)
                {
                    foreach (var child in jobNode.GetAllChildNodes())
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

    void MoveNodes(Model? model, double deltaX, double deltaY)
    {
        foreach (var (node, positions) in _initialPositions)
        {
            SetPosition(node, positions.position.X + deltaX, positions.position.Y + deltaY);
            deltaX = node.Position.X - positions.position.X;
            deltaY = node.Position.Y - positions.position.Y;

            foreach (var (childNode, childPosition) in positions.ChildPositions)
            {
                SetPosition(childNode, childPosition.X + deltaX, childPosition.Y + deltaY);
            }

            if (node is NodeModel movableNode)
            {
                movableNode.TriggerMoving();
            }
        }
    }

    void SetPosition(NodeModel node, double x, double y)
    {
        if (node is NodeModel nodeModel && nodeModel.ParentNode != null)
        {
            x = Clamp(x, nodeModel.Size?.Width, nodeModel.ParentNode.Position.X, nodeModel.ParentNode.Size?.Width);
            var parentY = nodeModel.ParentNode.Position.Y + CHILD_NODE_MIN_OFFSET_TOP;
            var parentH = nodeModel.ParentNode.Size?.Height - CHILD_NODE_MIN_OFFSET_TOP - CHILD_NODE_MIN_OFFSET_BOTTOM;
            y = Clamp(y, nodeModel.Size?.Height, parentY, parentH);

            nodeModel.SetPosition(x, y);
        }
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
    }
}

