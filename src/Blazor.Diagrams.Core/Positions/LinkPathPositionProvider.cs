using System.Linq;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models.Base;

namespace Blazor.Diagrams.Core.Positions;

public class LinkPathPositionProvider : IPositionProvider
{
    public LinkPathPositionProvider(double distance)
    {
        Distance = distance;
    }

    public double Distance { get; }

    public Point? GetPosition(Model model)
    {
        if (model is not BaseLinkModel link)
            throw new DiagramsException("LinkPathPositionProvider requires a link model");
        
        if (link.Paths.Length <= 0)
            return null;
            
        var totalLength = link.Paths.Sum(p => p.Length);

        var length = Distance switch
        {
            >= 0 and <= 1 => Distance * totalLength,
            > 1 => Distance,
            < 0 => totalLength + Distance
        };

        foreach (var path in link.Paths)
        {
            var pathLength = path.Length;
            if (length < pathLength)
            {
                var pt = path.GetPointAtLength(length);
                return new Point(pt.X, pt.Y);
            }

            length -= pathLength;
        }

        return null;

    }
}