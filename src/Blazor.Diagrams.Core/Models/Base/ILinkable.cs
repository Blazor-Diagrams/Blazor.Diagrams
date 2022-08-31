using System.Collections.Generic;

namespace Blazor.Diagrams.Core.Models.Base;

public interface ILinkable
{
    public IReadOnlyList<BaseLinkModel> Links { get; }

    public bool CanAttachTo(ILinkable other);

    public void Refresh();

    internal void AddLink(BaseLinkModel link);

    internal void RemoveLink(BaseLinkModel link);
}