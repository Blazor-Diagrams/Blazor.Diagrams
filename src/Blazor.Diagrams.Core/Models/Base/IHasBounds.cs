using Blazor.Diagrams.Core.Geometry;

namespace Blazor.Diagrams.Core.Models.Base;

public interface IHasBounds
{
    public Rectangle? GetBounds();
}