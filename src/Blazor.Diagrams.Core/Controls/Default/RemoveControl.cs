using System.Threading.Tasks;
using Blazor.Diagrams.Core.Events;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;
using Blazor.Diagrams.Core.Positions;

namespace Blazor.Diagrams.Core.Controls.Default;

public class RemoveControl : ExecutableControl
{
    private readonly IPositionProvider _positionProvider;

    public RemoveControl(double x, double y, double offsetX = 0, double offsetY = 0)
        : this(new BoundsBasedPositionProvider(x, y, offsetX, offsetY))
    {
    }

    public RemoveControl(IPositionProvider positionProvider)
    {
        _positionProvider = positionProvider;
    }

    public override Point? GetPosition(Model model) => _positionProvider.GetPosition(model);

    public override async ValueTask OnPointerDown(Diagram diagram, Model model, PointerEventArgs _)
    {
        switch (model)
        {

            case NodeModel node:
                if (await diagram.Options.Constraints.ShouldDeleteNode.Invoke(node))
                {
                    diagram.Nodes.Remove(node);
                }
                break;
            case BaseLinkModel link:
                if (await diagram.Options.Constraints.ShouldDeleteLink.Invoke(link))
                {
                    diagram.Links.Remove(link);
                }
                break;

            
        }
    }
}