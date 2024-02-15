﻿using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Blazor.Diagrams.Core.Models;

public class NodeModel : MovableModel, IHasBounds, IHasShape, ILinkable
{
    private readonly List<PortModel> _ports = new();
    private readonly List<BaseLinkModel> _links = new();
    private Size? _size;
    public Size MinimumDimensions { get; set; } = new Size(0, 0);

    public event Action<NodeModel>? SizeChanging;
    public event Action<NodeModel>? SizeChanged;
    public event Action<NodeModel>? Moving;

    public NodeModel(Point? position = null) : base(position)
    {
    }

    public NodeModel(string id, Point? position = null) : base(id, position)
    {
    }

    public Size? Size
    {
        get => _size;
        set
        {
            _size = value;
        }
    }
    public bool ControlledSize { get; init; }

    public GroupModel? Group { get; internal set; }
    public string? Title { get; set; }

    public IReadOnlyList<PortModel> Ports => _ports;
    public IReadOnlyList<BaseLinkModel> Links => _links;
    public IEnumerable<BaseLinkModel> PortLinks => Ports.SelectMany(p => p.Links);

    #region Ports

    public PortModel AddPort(PortModel port)
    {
        _ports.Add(port);
        return port;
    }

    public PortModel AddPort(PortAlignment alignment = PortAlignment.Bottom)
        => AddPort(new PortModel(this, alignment, Position));

    public PortModel? GetPort(PortAlignment alignment) => Ports.FirstOrDefault(p => p.Alignment == alignment);

    public T? GetPort<T>(PortAlignment alignment) where T : PortModel => (T?)GetPort(alignment);

    public bool RemovePort(PortModel port) => _ports.Remove(port);

    #endregion

    #region Refreshing

    public void RefreshAll()
    {
        Refresh();
        _ports.ForEach(p => p.RefreshAll());
    }

    public void RefreshLinks()
    {
        foreach (var link in Links)
        {
            link.Refresh();
            link.RefreshLinks();
        }
    }

    public void ReinitializePorts()
    {
        foreach (var port in Ports)
        {
            port.Initialized = false;
            port.Refresh();
        }
    }

    #endregion

    public override void SetPosition(double x, double y)
    {
        var deltaX = x - Position.X;
        var deltaY = y - Position.Y;
        base.SetPosition(x, y);

        UpdatePortPositions(deltaX, deltaY);
        Refresh();
        RefreshLinks();
        Moving?.Invoke(this);
    }

    public void SetSize(double width, double height)
    {
        var newSize = new Size(width, height);
        if (newSize.Equals(_size) == true || Size == null)
            return;
        var oldSize = new Size(Size.Width, Size.Height);

        Size = newSize;
        UpdatePortPositions(oldSize.Width, oldSize.Height, newSize.Width, newSize.Height);
        Refresh();
        RefreshLinks();
        SizeChanging?.Invoke(this);
    }

    public void TriggerSizeChanged()
    {
        SizeChanged?.Invoke(this);
    }

    public virtual void UpdatePositionSilently(double deltaX, double deltaY)
    {
        base.SetPosition(Position.X + deltaX, Position.Y + deltaY);
        UpdatePortPositions(deltaX, deltaY);
        Refresh();
    }

    public Rectangle? GetBounds() => GetBounds(false);

    public Rectangle? GetBounds(bool includePorts)
    {
        if (Size == null)
            return null;

        if (!includePorts)
            return new Rectangle(Position, Size);

        var leftPort = GetPort(PortAlignment.Left);
        var topPort = GetPort(PortAlignment.Top);
        var rightPort = GetPort(PortAlignment.Right);
        var bottomPort = GetPort(PortAlignment.Bottom);

        var left = leftPort == null ? Position.X : Math.Min(Position.X, leftPort.Position.X);
        var top = topPort == null ? Position.Y : Math.Min(Position.Y, topPort.Position.Y);
        var right = rightPort == null
            ? Position.X + Size!.Width
            : Math.Max(rightPort.Position.X + rightPort.Size.Width, Position.X + Size!.Width);
        var bottom = bottomPort == null
            ? Position.Y + Size!.Height
            : Math.Max(bottomPort.Position.Y + bottomPort.Size.Height, Position.Y + Size!.Height);

        return new Rectangle(left, top, right, bottom);
    }

    public virtual IShape GetShape() => Shapes.Rectangle(this);

    public virtual bool CanAttachTo(ILinkable other) => other is not PortModel && other is not BaseLinkModel;

    private void UpdatePortPositions(double deltaX, double deltaY)
    {
        // Save some JS calls and update ports directly here
        foreach (var port in _ports)
        {
            port.Position = new Point(port.Position.X + deltaX, port.Position.Y + deltaY);
            port.RefreshLinks();
        }
    }

    private void UpdatePortPositions(double oldWidth, double oldHeight, double newWidth, double newHeight)
    {
        foreach (var port in _ports)
        {
            switch (port.Alignment)
            {
                case PortAlignment.Top:
                    port.Position = new Point(port.Position.X - (oldWidth / 2) + (newWidth / 2), Position.Y);
                    break;
                case PortAlignment.TopRight:
                    port.Position = new Point(port.Position.X - (oldWidth) + (newWidth), port.Position.Y);
                    break;
                case PortAlignment.TopLeft:
                    port.Position = new Point(port.Position.X, port.Position.Y);
                    break;
                case PortAlignment.Right:
                    port.Position = new Point(port.Position.X - (oldWidth) + (newWidth), port.Position.Y - (oldHeight / 2) + (newHeight / 2));
                    break;
                case PortAlignment.Left:
                    port.Position = new Point(port.Position.X, port.Position.Y - (oldHeight / 2) + (newHeight / 2));
                    break;
                case PortAlignment.Bottom:
                    port.Position = new Point(port.Position.X - (oldWidth / 2) + (newWidth / 2), port.Position.Y - (oldHeight) + (newHeight));
                    break;
                case PortAlignment.BottomRight:
                    port.Position = new Point(port.Position.X - (oldWidth) + (newWidth), port.Position.Y - (oldHeight) + (newHeight));
                    break;
                case PortAlignment.BottomLeft:
                    port.Position = new Point(port.Position.X, port.Position.Y - (oldHeight) + (newHeight));
                    break;
            }
            port.RefreshLinks();
        }
    }

    protected void TriggerMoving()
    {
        Moving?.Invoke(this);
    }

    void ILinkable.AddLink(BaseLinkModel link) => _links.Add(link);

    void ILinkable.RemoveLink(BaseLinkModel link) => _links.Remove(link);
}