using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;

namespace SharedDemo.Demos.CustomPort;

public class ColoredPort : PortModel
{
    public ColoredPort(NodeModel parent, PortAlignment alignment, bool isRed) : base(parent, alignment, null, null)
    {
        IsRed = isRed;
    }

    public bool IsRed { get; set; }

    public override bool CanAttachTo(ILinkable other)
    {
        if (other is not PortModel port)
            return false;
        
        // Checks for same-node/port attachments
        if (!base.CanAttachTo(port))
            return false;

        // Only able to attach to the same port type
        if (port is not ColoredPort cp)
            return false;

        return IsRed == cp.IsRed;
    }
}
