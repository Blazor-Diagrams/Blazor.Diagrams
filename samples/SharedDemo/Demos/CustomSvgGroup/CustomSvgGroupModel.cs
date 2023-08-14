using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Models;

namespace SharedDemo.Demos.CustomSvgGroup;

public class CustomSvgGroupModel : SvgGroupModel
{
    public CustomSvgGroupModel(NodeModel[] children, string title, byte padding = 30) : base(children, padding)
    {
        Title = title;
    }
}
