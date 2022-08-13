using Blazor.Diagrams.Core.Models;
using System.Collections.Generic;

namespace Blazor.Diagrams.Models
{
    public class SvgGroupModel : GroupModel
    {
        public SvgGroupModel(IEnumerable<NodeModel> children, byte padding = 30) : base(children, padding) { }
    }
}
