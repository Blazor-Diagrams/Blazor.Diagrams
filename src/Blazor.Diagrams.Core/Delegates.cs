using Blazor.Diagrams.Core.Models.Base;
using Blazor.Diagrams.Core.Models.Core;

namespace Blazor.Diagrams.Core
{
    public delegate Point[] Router(DiagramManager diagram, BaseLinkModel link, Point from, Point to);

    public delegate PathGeneratorResult PathGenerator(DiagramManager diagram, BaseLinkModel link, Point[] route);
}
