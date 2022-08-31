using System;
using System.Linq;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;
using Blazor.Diagrams.Core.Positions;

namespace Blazor.Diagrams.Core.Anchors.Dynamic
{
    // Figure out a better name
    // Generic?
    public class DynamicAnchor : Anchor
    {
        public DynamicAnchor(NodeModel model, IPositionProvider[] providers, Point? offset = null) : base(model, offset)
        {
            if (providers.Length == 0)
                throw new InvalidOperationException("No providers provided");

            Providers = providers;
        }

        public IPositionProvider[] Providers { get; }

        public override Point? GetPosition(BaseLinkModel link, Point[] route)
        {
            var node = (Model as NodeModel)!;
            if (node.Size == null)
                return null;

            var isTarget = link.Target == this;
            var pt = route.Length > 0 ? route[isTarget ? ^1 : 0] : GetOtherPosition(link, isTarget);
            var positions = Providers.Select(e => e.GetPosition(node));
            return pt is null ? null : GetClosestPointTo(positions, pt);
        }

        public override Point? GetPlainPosition() => (Model as NodeModel)!.GetBounds()?.Center ?? null;
    }
}