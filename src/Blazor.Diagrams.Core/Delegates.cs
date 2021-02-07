using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Core;

namespace Blazor.Diagrams.Core
{
    public delegate Point[] Router(DiagramManager diagram, LinkModel link, Point from, Point to);

    public delegate string PathGenerator(DiagramManager diagram, LinkModel link, Point[] route);
}
