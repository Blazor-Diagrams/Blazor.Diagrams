using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;

namespace Blazor.Diagrams.Core
{
    public delegate Point[] Router(Diagram diagram, BaseLinkModel link);

    public delegate PathGeneratorResult PathGenerator(Diagram diagram, BaseLinkModel link, Point[] route, Point source, Point target);

    public delegate BaseLinkModel LinkFactory(Diagram diagram, PortModel sourcePort);

    public delegate GroupModel GroupFactory(Diagram diagram, NodeModel[] children);
}
