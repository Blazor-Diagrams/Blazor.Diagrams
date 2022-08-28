using System.Threading.Tasks;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;
using Blazor.Diagrams.Core.Positions;

namespace Blazor.Diagrams.Core.UserActions.Default;

public class RemoveUserAction : UserAction
{
    public RemoveUserAction(double x, double y, double offsetX = 0, double offsetY = 0)
        : base(new BoundsBasedPositionProvider(x, y, offsetX, offsetY))
    {
    }

    public RemoveUserAction(IPositionProvider positionProvider) : base(positionProvider)
    {
    }

    public override ValueTask Execute(Diagram diagram, Model model)
    {
        if (model is NodeModel node)
        {
            diagram.Nodes.Remove(node);
        }
        
        return ValueTask.CompletedTask;
    }
}