using Blazor.Diagrams.Core.Anchors;
using Blazor.Diagrams.Core.Geometry;
using System;
using System.Collections.Generic;

namespace Blazor.Diagrams.Core.Models.Base
{
    public abstract class BaseLinkModel : SelectableModel
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
        public bool IsAttached => Target != null;
        public Point? OnGoingPosition { get; set; }
        public Router? Router { get; set; }
        public PathGenerator? PathGenerator { get; set; }
        public LinkMarker? SourceMarker { get; set; }
        public LinkMarker? TargetMarker { get; set; }
        public bool Segmentable { get; set; } = false;
        public List<LinkVertexModel> Vertices { get; } = new List<LinkVertexModel>();
        public List<LinkLabelModel> Labels { get; set; } = new List<LinkLabelModel>();

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
    }
}
