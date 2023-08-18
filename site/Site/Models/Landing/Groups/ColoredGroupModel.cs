using Blazor.Diagrams.Core.Models;

namespace Site.Models.Landing.Groups;

public class ColoredGroupModel : GroupModel
{
    public ColoredGroupModel(IEnumerable<NodeModel> children, string color, byte padding = 30, bool autoSize = true) : base(children, padding, autoSize)
    {
        Color = color;
    }

    public string Color { get; }
}
