using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;

namespace Blazor.Diagrams.Core
{
    public delegate Point[] Router(DiagramBase diagram, BaseLinkModel link);

    public delegate PathGeneratorResult PathGenerator(DiagramBase diagram, BaseLinkModel link, Point[] route, Point source, Point target);

    public delegate BaseLinkModel LinkFactory(DiagramBase diagram, PortModel sourcePort);

    public delegate GroupModel GroupFactory(DiagramBase diagram, NodeModel[] children);
}
