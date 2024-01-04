using Blazor.Diagrams.Core.Events;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models.Base;
using Blazor.Diagrams.Core.Positions.Resizing;
using System.Threading.Tasks;

namespace Blazor.Diagrams.Core.Controls.Default
{
    public class ResizeControl : ExecutableControl
    {
        private readonly ResizerProvider _resizeProvider;

        public ResizeControl(ResizerProvider resizeProvider)
        {
            _resizeProvider = resizeProvider;
        }

        public override Point? GetPosition(Model model) => _resizeProvider.GetPosition(model);

        public string? Class => _resizeProvider.Class;

        public override ValueTask OnPointerDown(Diagram diagram, Model model, PointerEventArgs e)
        {
            _resizeProvider.OnResizeStart(diagram, model, e);
            diagram.PointerMove += _resizeProvider.OnPointerMove;
            diagram.PanChanged += _resizeProvider.OnPanChanged;
            diagram.PointerUp += _resizeProvider.OnResizeEnd;
            diagram.PointerUp += (_, _) => OnResizeEnd(diagram);

            return ValueTask.CompletedTask;
        }

        void OnResizeEnd(Diagram diagram)
        {
            diagram.PointerMove -= _resizeProvider.OnPointerMove;
            diagram.PanChanged -= _resizeProvider.OnPanChanged;
            diagram.PointerUp -= _resizeProvider.OnResizeEnd;
        }
    }
}
