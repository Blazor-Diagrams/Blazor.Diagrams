using Blazor.Diagrams.Core.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazor.Diagrams.Core.Behaviors;

public interface IDragNewLinkBehavior : IBehavior
{
    BaseLinkModel? OngoingLink { get; }
    void StartFrom(ILinkable source, double clientX, double clientY);
}
