using Blazor.Diagrams.Core.Anchors;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;

namespace Blazor.Diagrams.Core
{
    public delegate Point[] Router(Diagram diagram, BaseLinkModel link);

    public delegate PathGeneratorResult PathGenerator(Diagram diagram, BaseLinkModel link, Point[] route, Point source, Point target);

    public delegate BaseLinkModel? LinkFactory(Diagram diagram, ILinkable source, Anchor targetAnchor);

    public delegate Anchor AnchorFactory(Diagram diagram, BaseLinkModel link, ILinkable model);

    public delegate GroupModel GroupFactory(Diagram diagram, NodeModel[] children);
}
