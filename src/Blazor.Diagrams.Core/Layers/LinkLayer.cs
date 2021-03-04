using Blazor.Diagrams.Core.Models.Base;

namespace Blazor.Diagrams.Core.Layers
{
    public class LinkLayer : BaseLayer<BaseLinkModel>
    {
        public LinkLayer(Diagram diagram) : base(diagram) { }

        protected override void OnItemAdded(BaseLinkModel link)
        {
            link.SourcePort.AddLink(link);
            link.TargetPort?.AddLink(link);

            link.SourcePort.Refresh();
            link.TargetPort?.Refresh();

            link.SourcePort.Parent.Group?.Refresh();
            link.TargetPort?.Parent.Group?.Refresh();
        }

        protected override void OnItemRemoved(BaseLinkModel link)
        {
            link.SourcePort.RemoveLink(link);
            link.TargetPort?.RemoveLink(link);

            link.SourcePort.Refresh();
            link.TargetPort?.Refresh();

            link.SourcePort.Parent.Group?.Refresh();
            link.TargetPort?.Parent.Group?.Refresh();
        }
    }
}
