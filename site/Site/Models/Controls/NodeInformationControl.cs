using Blazor.Diagrams.Core.Controls;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;

namespace Site.Models.Controls;

public class NodeInformationControl : Control
{
    public override Point? GetPosition(Model model)
    {
        // We want the information to be under the node
        var node = (model as NodeModel)!;
        if (node.Size == null)
            return null;

        return node.Position.Add(0, node.Size!.Height + 10);
    }
}
