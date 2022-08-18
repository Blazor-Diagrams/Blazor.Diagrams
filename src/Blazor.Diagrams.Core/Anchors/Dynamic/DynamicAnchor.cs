using System;
using System.Collections.Generic;
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
        public DynamicAnchor(NodeModel node, IPositionProvider[] providers, Point? offset = null)
            : base(node, offset)
        {
            if (providers.Length == 0)
                throw new InvalidOperationException("No providers provided");

            Providers = providers;
        }

        public IPositionProvider[] Providers { get; }

        public override Point? GetPosition(BaseLinkModel link, Point[] route)
        {
            if (Node.Size == null)
                return null;

            var isTarget = link.Target == this;
            var pt = route.Length > 0 ? route[isTarget ? ^1 : 0] : GetOtherPosition(link, isTarget);
            var positions = Providers.Select(e => e.GetPosition(Node));
            return pt is null ? null : GetClosestPointTo(positions, pt);
        }
    }
}