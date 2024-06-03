namespace Blazor.Diagrams.Core.Models.Base;

public interface IHasChild
{
    public List<NodeModel> GetAllChildNodes();

    internal void AddChildNode(NodeModel child);

    internal void RemoveChildNode(NodeModel child);

    internal void ClearChildNodes();
}