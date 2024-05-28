using Blazor.Diagrams.Core.Geometry;
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

    internal NodeModel? ParentNode;

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
        if (newSize.Equals(_size) == true)
            return;

        Size? oldSize = Size != null ? new Size(Size.Width, Size.Height) : null;

        Size = newSize;
        if (oldSize != null)
        {
            UpdatePortPositions(oldSize, newSize);
        }
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

    /// <summary>
    /// Updates port positions when node position changes.
    /// </summary>
    private void UpdatePortPositions(double deltaX, double deltaY)
    {
        // Save some JS calls and update ports directly here
        foreach (var port in _ports)
        {
            port.Position = new Point(port.Position.X + deltaX, port.Position.Y + deltaY);
            port.RefreshLinks();
        }
    }

    /// <summary>
    /// Updates port positions when node size changes.
    /// </summary>
    private void UpdatePortPositions(Size oldSize, Size newSize)
    {
        var deltaWidth = newSize.Width - oldSize.Width;
        var deltaHeight = newSize.Height - oldSize.Height;

        foreach (var port in _ports)
        {
            port.SetPortPositionOnNodeSizeChanged(deltaWidth, deltaHeight);
            port.RefreshLinks();
        }
    }

    internal void TriggerMoving()
    {
        Moving?.Invoke(this);
    }

    void ILinkable.AddLink(BaseLinkModel link) => _links.Add(link);

    void ILinkable.RemoveLink(BaseLinkModel link) => _links.Remove(link);

    internal List<NodeModel> GetAllChildNodes()
    {
        var allChildren = new List<NodeModel>();
        foreach (var child in childMovableNodes)
        {
            allChildren.Add(child);
            if (child is NodeModel jobChild)
            {
                allChildren.AddRange(jobChild.GetAllChildNodes());
            }
        }
        return allChildren;
    }

    internal void AddChildNode(NodeModel child)
    {
        child.ParentNode = this;
        childMovableNodes.Add(child);
    }

    internal void RemoveChildNode(NodeModel child)
    {
        childMovableNodes.Remove(child);
        child.ParentNode = null;
    }

    internal List<NodeModel> childMovableNodes = new List<NodeModel>();
}