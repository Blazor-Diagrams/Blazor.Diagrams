using System.Threading.Tasks;
using Blazor.Diagrams.Core.Events;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models.Base;
using Blazor.Diagrams.Core.Positions;

namespace Blazor.Diagrams.Core.Controls;

public abstract class ExecutableControl : Control
{
    public IPositionProvider PositionProvider { get; }

    protected ExecutableControl(IPositionProvider positionProvider)
    {
        PositionProvider = positionProvider;
    }

    public override Point? GetPosition(Model model) => PositionProvider.GetPosition(model);

    public abstract ValueTask OnPointerDown(Diagram diagram, Model model, PointerEventArgs e);
}