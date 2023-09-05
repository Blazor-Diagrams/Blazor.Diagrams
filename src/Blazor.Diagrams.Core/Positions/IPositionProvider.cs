using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models.Base;

namespace Blazor.Diagrams.Core.Positions;

public interface IPositionProvider
{
    public Point? GetPosition(Model model);
}