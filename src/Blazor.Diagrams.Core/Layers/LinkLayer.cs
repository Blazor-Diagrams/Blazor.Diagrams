using Blazor.Diagrams.Core.Anchors;
using Blazor.Diagrams.Core.Anchors.Dynamic;
using Blazor.Diagrams.Core.Models.Base;

namespace Blazor.Diagrams.Core.Layers
{
    public class LinkLayer : BaseLayer<BaseLinkModel>
    {
        public LinkLayer(DiagramBase diagram) : base(diagram) { }

        protected override void OnItemAdded(BaseLinkModel link)
        {
            link.Diagram = Diagram;
            HandleAnchor(link, link.Source, true);
            if (link.Target != null) HandleAnchor(link, link.Target, true);
            link.Refresh();

            link.Source.Node.Group?.Refresh();
            link.Target?.Node.Group?.Refresh();

            link.SourceChanged += OnLinkSourceChanged;
            link.TargetChanged += OnLinkTargetChanged;
        }

        protected override void OnItemRemoved(BaseLinkModel link)
        {
            link.Diagram = null;
            HandleAnchor(link, link.Source, false);
            if (link.Target != null) HandleAnchor(link, link.Target, false);
            link.Refresh();

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
            else if (anchor is ShapeIntersectionAnchor or DynamicAnchor)
            {
                if (add)
                {
                    anchor.Node.AddLink(link);
                }
                else
                {
                    anchor.Node.RemoveLink(link);
                }
            }
            else
            {
                throw new DiagramsException($"Unhandled Anchor type {anchor.GetType().Name}");
            }
        }
    }
}
