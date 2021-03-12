using Blazor.Diagrams.Core.Models.Base;

namespace Blazor.Diagrams.Core.Layers
{
    public class LinkLayer : BaseLayer<BaseLinkModel>
    {
        public LinkLayer(Diagram diagram) : base(diagram) { }

        protected override void OnItemAdded(BaseLinkModel link)
        {
            if (!link.IsPortless)
            {
                link.SourcePort!.AddLink(link);
                link.TargetPort?.AddLink(link);

                link.SourcePort.Refresh();
                link.TargetPort?.Refresh();
            }
            else
            {
                link.SourceNode.AddLink(link);
                link.TargetNode?.AddLink(link);
            }

            link.SourceNode.Group?.Refresh();
            link.TargetNode?.Group?.Refresh();
        }

        protected override void OnItemRemoved(BaseLinkModel link)
        {
            if (!link.IsPortless)
            {
                link.SourcePort!.RemoveLink(link);
                link.TargetPort?.RemoveLink(link);

                link.SourcePort.Refresh();
                link.TargetPort?.Refresh();
            }
            else
            {
                link.SourceNode.AddLink(link);
                link.TargetNode?.AddLink(link);
            }

            link.SourceNode.Group?.Refresh();
            link.TargetNode?.Group?.Refresh();
        }
    }
}
