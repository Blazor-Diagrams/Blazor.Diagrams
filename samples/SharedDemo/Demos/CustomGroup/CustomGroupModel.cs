using Blazor.Diagrams.Core.Models;

namespace SharedDemo.Demos.CustomGroup;

public class CustomGroupModel : GroupModel
{
    public CustomGroupModel(NodeModel[] children, string title, byte padding = 30)
        : base(children, padding)
    {
        Title = title;
    }

    public string Title { get; }
}
