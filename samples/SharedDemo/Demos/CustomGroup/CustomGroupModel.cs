using Blazor.Diagrams.Core;
using Blazor.Diagrams.Core.Models;

namespace SharedDemo.Demos.CustomGroup
{
    public class CustomGroupModel : GroupModel
    {
        public CustomGroupModel(DiagramManager diagramManager, NodeModel[] children, string title, byte padding = 30)
            : base(diagramManager, children, padding)
        {
            Title = title;
        }

        public string Title { get; }
    }
}
