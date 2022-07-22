using Blazor.Diagrams.Core.Models;
using System.Linq;

namespace Blazor.Diagrams.Core.Layers
{
    public class NodeLayer : BaseLayer<NodeModel>
    {
        public NodeLayer(DiagramBase diagram) : base(diagram) { }

        public override void Remove(NodeModel node)
        {
            Diagram.Batch(() => base.Remove(node));
        }

        protected override void OnItemRemoved(NodeModel node)
        {
            Diagram.Links.Remove(node.AllLinks.ToList());
            Diagram.Links.Remove(node.Links.ToList());
            node.Group?.RemoveChild(node);
        }
    }
}
