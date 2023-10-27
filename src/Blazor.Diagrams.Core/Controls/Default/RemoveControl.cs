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
        if (await ShouldDeleteModel(diagram, model))
        {
            DeleteModel(diagram, model);
        }
    }

    private static void DeleteModel(Diagram diagram, Model model)
    {
       switch (model)
        {
            case GroupModel group:
                diagram.Groups.Delete(group);
                return;
            case NodeModel node:
                diagram.Nodes.Remove(node);
                return;

            case BaseLinkModel link:
                diagram.Links.Remove(link);
                return;
        }
    }

    private static async ValueTask<bool> ShouldDeleteModel(Diagram diagram, Model model)
    {
        if (model.Locked)
        {
            return false;
        }

        return model switch
        {
            GroupModel group => await diagram.Options.Constraints.ShouldDeleteGroup.Invoke(group),
            NodeModel node => await diagram.Options.Constraints.ShouldDeleteNode.Invoke(node),
            BaseLinkModel link => await diagram.Options.Constraints.ShouldDeleteLink.Invoke(link),
            _ => false,
        };
    }
}