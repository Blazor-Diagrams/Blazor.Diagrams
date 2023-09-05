using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models.Base;

namespace Blazor.Diagrams.Core.Controls;

public abstract class Control
{
    public abstract Point? GetPosition(Model model);
}