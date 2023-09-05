using Blazor.Diagrams.Core.Models.Base;
using Blazor.Diagrams.Models;

namespace Blazor.Diagrams.Extensions;

public static class ModelExtensions
{
    public static bool IsSvg(this Model model)
    {
        return model is SvgNodeModel or SvgGroupModel or BaseLinkModel;
    }
}