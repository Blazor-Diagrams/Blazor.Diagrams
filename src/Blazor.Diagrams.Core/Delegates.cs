using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;
using Blazor.Diagrams.Core.Models.Core;

namespace Blazor.Diagrams.Core
{
    public delegate Point[] Router(Diagram diagram, BaseLinkModel link, Point from, Point to);

    public delegate PathGeneratorResult PathGenerator(Diagram diagram, BaseLinkModel link, Point[] route);

    public delegate BaseLinkModel LinkFactory(Diagram diagram, PortModel sourcePort);

    public delegate GroupModel GroupFactory(Diagram diagram, NodeModel[] children);
}
