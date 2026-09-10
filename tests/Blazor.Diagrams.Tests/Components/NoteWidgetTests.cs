using Blazor.Diagrams.Components;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;

using Bunit;

using FluentAssertions;

using Xunit;

namespace Blazor.Diagrams.Tests.Components;

public class NoteWidgetTests
{
    [Fact]
    public void NoteModel_ShouldRenderNoteWidgetByDefault()
    {
        var diagram = new BlazorDiagram();
        var note = new NoteModel("Hello", new Point(10, 20));
        note.Selected = true;

        diagram.GetComponent(note).Should().Be(typeof(NoteWidget));
        using var ctx = new TestContext();
        var cut = ctx.RenderComponent<NoteWidget>(parameters => parameters.Add(n => n.Node, note));

        var content = cut.Find("div.default-note");
        content.TextContent.Should().Be("Hello");
        content.ClassList.Should().Contain("selected");
        note.CanAttachTo(new NodeModel()).Should().BeFalse();
    }

    [Fact]
    public void NoteWidget_ShouldEditTextOnDoubleClick()
    {
        using var ctx = new TestContext();
        var note = new NoteModel("Hello");
        var cut = ctx.RenderComponent<NoteWidget>(parameters => parameters.Add(n => n.Node, note));
        cut.FindAll("textarea").Should().BeEmpty();

        cut.Find("div.default-note").DoubleClick();
        var textarea = cut.Find("textarea");
        textarea.Input("Changed");
        textarea.Blur();

        note.Text.Should().Be("Changed");
        cut.FindAll("textarea").Should().BeEmpty();
        cut.Find("div.default-note").TextContent.Should().Be("Changed");
    }
}
