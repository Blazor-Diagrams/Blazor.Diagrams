using System.Threading.Tasks;
using Blazor.Diagrams.Core.Models.Base;
using Blazor.Diagrams.Core.Positions;

namespace Blazor.Diagrams.Core.UserActions;

public abstract class UserAction
{
    protected UserAction(IPositionProvider positionProvider)
    {
        PositionProvider = positionProvider;
    }

    public IPositionProvider PositionProvider { get; }

    public abstract ValueTask Execute(Diagram diagram, Model model);
}