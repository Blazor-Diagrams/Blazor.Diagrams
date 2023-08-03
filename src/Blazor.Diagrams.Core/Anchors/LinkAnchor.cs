using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models.Base;
using Blazor.Diagrams.Core.Positions;

namespace Blazor.Diagrams.Core.Anchors;

public class LinkAnchor : Anchor
{
    private readonly LinkPathPositionProvider _positionProvider;

    public LinkAnchor(BaseLinkModel link, double distance, double offsetX = 0, double offsetY = 0) : base(link)
    {
        _positionProvider = new LinkPathPositionProvider(distance, offsetX, offsetY);
        Link = link;
    }

    public BaseLinkModel Link { get; }

    public override Point? GetPosition(BaseLinkModel link, Point[] route) => _positionProvider.GetPosition(Link);

    public override Point? GetPlainPosition() => _positionProvider.GetPosition(Link);
}