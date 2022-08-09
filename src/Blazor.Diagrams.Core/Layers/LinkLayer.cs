using Blazor.Diagrams.Core.Anchors;
using Blazor.Diagrams.Core.Models.Base;

namespace Blazor.Diagrams.Core.Layers
{
    public class LinkLayer : BaseLayer<BaseLinkModel>
    {
        public LinkLayer(DiagramBase diagram) : base(diagram) { }

        protected override void OnItemAdded(BaseLinkModel link)
        {
            HandleAnchor(link, link.Source, true);
            if (link.Target != null) HandleAnchor(link, link.Target, true);

            link.Source.Node.Group?.Refresh();
            link.Target?.Node.Group?.Refresh();

            link.SourceChanged += OnLinkSourceChanged;
            link.TargetChanged += OnLinkTargetChanged;
        }

        protected override void OnItemRemoved(BaseLinkModel link)
        {
            HandleAnchor(link, link.Source, false);
            if (link.Target != null) HandleAnchor(link, link.Target, false);

            link.Source.Node.Group?.Refresh();
            link.Target?.Node.Group?.Refresh();

            link.SourceChanged -= OnLinkSourceChanged;
            link.TargetChanged -= OnLinkTargetChanged;
        }

        private void OnLinkSourceChanged(BaseLinkModel link, Anchor old, Anchor @new)
        {
            HandleAnchor(link, old, add: false);
            HandleAnchor(link, @new, add: true);
        }

        private void OnLinkTargetChanged(BaseLinkModel link, Anchor? old, Anchor? @new)
        {
            if (old != null) HandleAnchor(link, old, add: false);
            if (@new != null) HandleAnchor(link, @new, add: true);
        }

        private static void HandleAnchor(BaseLinkModel link, Anchor anchor, bool add)
        {
            if (anchor is SinglePortAnchor spa)
            {
                if (add)
                {
                    spa.Port.AddLink(link);
                }
                else
                {
                    spa.Port.RemoveLink(link);
                }

                spa.Port.Refresh();
            }
            else if (anchor is ShapeIntersectionAnchor sia)
            {
                if (add)
                {
                    sia.Node.AddLink(link);
                }
                else
                {
                    sia.Node.RemoveLink(link);
                }
            }
            else
            {
                throw new DiagramsException($"Unhandled Anchor type {anchor.GetType().Name}");
            }
        }
    }
}
