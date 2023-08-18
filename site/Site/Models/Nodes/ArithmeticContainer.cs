using Blazor.Diagrams.Core.Models;

namespace Site.Models.Nodes;

public class ArithmeticContainer : GroupModel
{
    public ArithmeticContainer(IEnumerable<NodeModel> children, byte padding = 30, bool autoSize = true) : base(children, padding, autoSize)
    {
    }
}
