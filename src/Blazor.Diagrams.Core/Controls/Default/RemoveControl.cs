using System.Threading.Tasks;
using Blazor.Diagrams.Core.Events;
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

    public override ValueTask OnPointerDown(Diagram diagram, Model model, PointerEventArgs _)
    {
        switch (model)
        {
            case NodeModel node:
                diagram.Nodes.Remove(node);
                break;
            case BaseLinkModel link:
                diagram.Links.Remove(link);
                break;
        }

        return ValueTask.CompletedTask;
    }
}