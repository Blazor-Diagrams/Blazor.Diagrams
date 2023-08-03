using Blazor.Diagrams.Core.Geometry;

namespace Blazor.Diagrams.Core.Models.Base;

public interface IHasShape
{
    public IShape GetShape();
}