using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models.Base;

namespace Blazor.Diagrams.Core.Models;

/// <summary>
/// A free-floating piece of text on the diagram. Behaves like a node (movable, selectable) but can't be linked.
/// </summary>
public class NoteModel : NodeModel
{
    public NoteModel(string text, Point? position = null) : base(position)
    {
        Text = text;
    }

    public NoteModel(string id, string text, Point? position = null) : base(id, position)
    {
        Text = text;
    }

    public string Text { get; set; }

    public override bool CanAttachTo(ILinkable other) => false;
}
