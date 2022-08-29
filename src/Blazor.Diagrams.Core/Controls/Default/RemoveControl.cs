using System.Threading.Tasks;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;
using Blazor.Diagrams.Core.Positions;

namespace Blazor.Diagrams.Core.Controls.Default;

public class RemoveControl : ExecutableControl
{
    public RemoveControl(double x, double y, double offsetX = 0, double offsetY = 0)
        : base(new BoundsBasedPositionProvider(x, y, offsetX, offsetY))
    {
    }

    public RemoveControl(IPositionProvider positionProvider) : base(positionProvider)
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