using Blazor.Diagrams;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;

namespace SharedDemo.Demos.Nodes;

public partial class Notes
{
    private readonly BlazorDiagram _blazorDiagram = new();

    protected override void OnInitialized()
    {
        base.OnInitialized();

        LayoutData.Title = "Notes";
        LayoutData.Info = "Notes are free-floating pieces of text. They can be moved and selected like nodes, but can't be linked. " +
                          "Register your own component for NoteModel to change how they look.";
        LayoutData.DataChanged();

        var node1 = new NodeModel(new Point(80, 80));
        var node2 = new NodeModel(new Point(380, 200));
        _blazorDiagram.Nodes.Add(new[] { node1, node2 });
        _blazorDiagram.Links.Add(new LinkModel(node1, node2));

        _blazorDiagram.Nodes.Add(new NoteModel("Drag me around!\nDouble-click to edit.", new Point(250, 40)));
        _blazorDiagram.Nodes.Add(new NoteModel("Notes support\nmultiple lines.", new Point(100, 250)));
    }
}
