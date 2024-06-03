namespace Blazor.Diagrams.Core.Models.Base;

public interface IHasParent
{
    public NodeModel GetParentNode();
}