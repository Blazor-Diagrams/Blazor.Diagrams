using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;

namespace Site.Models.Landing;

public class CalculatorPortModel : PortModel
{
    public CalculatorPortModel(NodeModel parent, bool input) : base(parent,
        input ? PortAlignment.Left : PortAlignment.Right)
    {
        Input = input;
    }

    public bool Input { get; }

    public override bool CanAttachTo(ILinkable other)
    {
        if (other is not CalculatorPortModel port)
            return false;

        return port.Input != Input;
    }
}