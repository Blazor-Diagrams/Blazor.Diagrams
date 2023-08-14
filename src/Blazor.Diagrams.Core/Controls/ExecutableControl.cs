using Blazor.Diagrams.Core.Events;
using Blazor.Diagrams.Core.Models.Base;
using System.Threading.Tasks;

namespace Blazor.Diagrams.Core.Controls;

public abstract class ExecutableControl : Control
{
    public abstract ValueTask OnPointerDown(Diagram diagram, Model model, PointerEventArgs e);
}