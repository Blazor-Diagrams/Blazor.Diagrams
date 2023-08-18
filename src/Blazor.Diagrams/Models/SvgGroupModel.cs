using System.Collections.Generic;
using Blazor.Diagrams.Core.Models;

namespace Blazor.Diagrams.Models;

public class SvgGroupModel : GroupModel
{
    public SvgGroupModel(IEnumerable<NodeModel> children, byte padding = 30, bool autoSize = true) : base(children, padding, autoSize)
    {
    }
}