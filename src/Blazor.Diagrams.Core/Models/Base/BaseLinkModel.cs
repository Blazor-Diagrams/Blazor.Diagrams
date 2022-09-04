using Blazor.Diagrams.Core.Anchors;
using Blazor.Diagrams.Core.Geometry;
using System;
using System.Collections.Generic;
using SvgPathProperties;

namespace Blazor.Diagrams.Core.Models.Base
{
    public abstract class BaseLinkModel : SelectableModel, IHasBounds
    {
        public event Action<BaseLinkModel, Anchor, Anchor>? SourceChanged;
        public event Action<BaseLinkModel, Anchor?, Anchor?>? TargetChanged;

        protected BaseLinkModel(Anchor source, Anchor? target = null)
        {
            Source = source;
            Target = target;
        }

        protected BaseLinkModel(string id, Anchor source, Anchor? target = null) : base(id)
        {
            Source = source;
            Target = target;
        }

        public Anchor Source { get; private set; }
        public Anchor? Target { get; private set; }
        public Diagram? Diagram { get; internal set; }
        public PathGeneratorResult GeneratedPathResult { get; private set; } = PathGeneratorResult.Empty;
        public SvgPath[] Paths => GeneratedPathResult.Paths;
        public bool IsAttached => Target != null;
        public Point? OnGoingPosition { get; set; }
        public Router? Router { get; set; }
        public PathGenerator? PathGenerator { get; set; }
        public LinkMarker? SourceMarker { get; set; }
        public LinkMarker? TargetMarker { get; set; }
        public bool Segmentable { get; set; } = false;
        public List<LinkVertexModel> Vertices { get; } = new();
        public List<LinkLabelModel> Labels { get; } = new();

        public override void Refresh()
        {
            GeneratePath();
            base.Refresh();
        }

        public void SetSource(Anchor anchor)
        {
            ArgumentNullException.ThrowIfNull(anchor, nameof(anchor));

            if (Source == anchor)
                return;

            var old = Source;
            Source = anchor;
            SourceChanged?.Invoke(this, old, Source);
        }

        public void SetTarget(Anchor? anchor)
        {
            if (Target == anchor)
                return;

            var old = Target;
            Target = anchor;
            TargetChanged?.Invoke(this, old, Target);
        }

        public Rectangle? GetBounds()
        {
            if (Paths.Length == 0)
                return Rectangle.Zero;

            var minX = double.PositiveInfinity;
            var minY = double.PositiveInfinity;
            var maxX = double.NegativeInfinity;
            var maxY = double.NegativeInfinity;

            foreach (var path in Paths)
            {
                var bbox = path.GetBBox();
                minX = Math.Min(minX, bbox.Left);
                minY = Math.Min(minY, bbox.Top);
                maxX = Math.Max(maxX, bbox.Right);
                maxY = Math.Max(maxY, bbox.Bottom);
            }

            return new Rectangle(minX, minY, maxX, maxY);
        }

        private void GeneratePath()
        {
            if (Diagram != null)
            {
                var router = Router ?? Diagram.Options.Links.DefaultRouter;
                var pathGenerator = PathGenerator ?? Diagram.Options.Links.DefaultPathGenerator;
                var route = router(Diagram, this);
                var source = Source.GetPosition(this, route);
                var target = Target is null ? OnGoingPosition : Target.GetPosition(this, route);
                if (source != null && target != null)
                {
                    GeneratedPathResult = pathGenerator(Diagram, this, route, source, target);
                    return;
                }
            }

            GeneratedPathResult = PathGeneratorResult.Empty;
        }
    }
}
