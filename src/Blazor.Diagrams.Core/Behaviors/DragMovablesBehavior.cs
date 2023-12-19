using Blazor.Diagrams.Core.Behaviors.Base;
using Blazor.Diagrams.Core.Events;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;
using System;
using System.Collections.Generic;

namespace Blazor.Diagrams.Core.Behaviors;

public class DragMovablesBehavior : Behavior
{
    private readonly Dictionary<MovableModel, Point> _initialPositions;
    private double? _lastClientX;
    private double? _lastClientY;
    private bool _moved;
    private double _totalMovedX = 0;
    private double _totalMovedY = 0;

    public DragMovablesBehavior(Diagram diagram) : base(diagram)
    {
        _initialPositions = new Dictionary<MovableModel, Point>();
        Diagram.PointerDown += OnPointerDown;
        Diagram.PointerMove += OnPointerMove;
        Diagram.PointerUp += OnPointerUp;
        Diagram.Wheel += OnWheel;
    }

    private void OnPointerDown(Model? model, PointerEventArgs e)
    {
        if (model is not MovableModel)
            return;

        _initialPositions.Clear();
        foreach (var sm in Diagram.GetSelectedModels())
        {
            if (sm is not MovableModel movable || movable.Locked)
                continue;

            // Special case: groups without auto size on
            if (sm is NodeModel node && node.Group != null && !node.Group.AutoSize)
                continue;

            var position = movable.Position;
            if (Diagram.Options.GridSnapToCenter && movable is NodeModel n)
            {
                position = new Point(movable.Position.X + (n.Size?.Width ?? 0) / 2,
                    movable.Position.Y + (n.Size?.Height ?? 0) / 2);
            }

            _initialPositions.Add(movable, position);
        }

        _lastClientX = e.ClientX;
        _lastClientY = e.ClientY;
        _moved = false;
    }

    private void OnPointerMove(Model? model, PointerEventArgs e)
    {
        if (_initialPositions.Count == 0 || _lastClientX == null || _lastClientY == null)
            return;

        _moved = true;
        var deltaX = (e.ClientX - _lastClientX.Value) / Diagram.Zoom;
        var deltaY = (e.ClientY - _lastClientY.Value) / Diagram.Zoom;

        _totalMovedX += deltaX;
        _totalMovedY += deltaY;

        MoveNodes(model, _totalMovedX, _totalMovedY);

        _lastClientX = e.ClientX;
        _lastClientY = e.ClientY;

    }

    public void OnWheel(WheelEventArgs e)
    {
        if (_initialPositions.Count == 0 || _lastClientX == null || _lastClientY == null)
            return;

        _moved = true;

        _totalMovedX += e.DeltaX;
        _totalMovedY += e.DeltaY;

        MoveNodes(null, _totalMovedX, _totalMovedY);
    }

    private void MoveNodes(Model? model, double deltaX, double deltaY)
    {
        foreach (var (movable, initialPosition) in _initialPositions)
        {
            var ndx = ApplyGridSize(deltaX + initialPosition.X);
            var ndy = ApplyGridSize(deltaY + initialPosition.Y);
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

    private void OnPointerUp(Model? model, PointerEventArgs e)
    {
        if (_initialPositions.Count == 0)
            return;

        if (_moved)
        {
            foreach (var (movable, _) in _initialPositions)
            {
                movable.TriggerMoved();
            }
        }
        _initialPositions.Clear();
        _totalMovedX = 0;
        _totalMovedY = 0;
        _lastClientX = null;
        _lastClientY = null;
    }

    private double ApplyGridSize(double n)
    {
        if (Diagram.Options.GridSize == null)
            return n;

        var gridSize = Diagram.Options.GridSize.Value;
        return gridSize * Math.Floor((n + gridSize / 2.0) / gridSize);
    }

    public override void Dispose()
    {
        _initialPositions.Clear();
        Diagram.PointerDown -= OnPointerDown;
        Diagram.PointerMove -= OnPointerMove;
        Diagram.PointerUp -= OnPointerUp;
        Diagram.Wheel -= OnWheel;
    }
}
