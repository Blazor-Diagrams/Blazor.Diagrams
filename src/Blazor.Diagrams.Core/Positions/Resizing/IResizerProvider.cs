using Blazor.Diagrams.Core.Events;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;

namespace Blazor.Diagrams.Core.Positions.Resizing
{
    public interface IResizerProvider : IPositionProvider
    {
        public string? Class { get; }
        public void OnResizeStart(Diagram diagram, Model model, PointerEventArgs eventArgs);
        public void OnPointerMove(Model? model, PointerEventArgs args);
        public void OnResizeEnd(Model? model, PointerEventArgs args);
    }
}
