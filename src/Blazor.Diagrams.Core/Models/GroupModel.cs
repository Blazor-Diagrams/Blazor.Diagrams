using Blazor.Diagrams.Core.Extensions;
using Blazor.Diagrams.Core.Models.Base;
using Blazor.Diagrams.Core.Models.Core;
using System.Collections.Generic;
using System.Linq;

namespace Blazor.Diagrams.Core.Models
{
    public class GroupModel : NodeModel
    {
        public GroupModel(NodeModel[] children, byte padding = 30)
        {
            Size = Size.Zero;
            Children = children;
            Padding = padding;
            Initialize();
        }

        public NodeModel[] Children { get; private set; }
        public byte Padding { get; }

        public IEnumerable<BaseLinkModel> HandledLinks => Children.SelectMany(c => c.AllLinks).Distinct();

        public override void SetPosition(double x, double y)
        {
            var deltaX = x - Position.X;
            var deltaY = y - Position.Y;
            base.SetPosition(x, y);

            foreach (var node in Children)
                node.UpdatePositionSilently(deltaX, deltaY);

            Refresh();
        }

        public override void UpdatePositionSilently(double deltaX, double deltaY)
        {
            base.UpdatePositionSilently(deltaX, deltaY);

            foreach (var child in Children)
                child.UpdatePositionSilently(deltaX, deltaY);
        }

        public void Ungroup()
        {
            foreach (var child in Children)
            {
                child.Group = null;
                child.SizeChanged -= OnNodeChanged;
                child.Moving -= OnNodeChanged;
            }
        }

        private void Initialize()
        {
            foreach (var child in Children)
            {
                child.Group = this;
                child.SizeChanged += OnNodeChanged;
                child.Moving += OnNodeChanged;
            }

            UpdateDimensions();
        }

        private void OnNodeChanged(NodeModel node)
        {
            if (UpdateDimensions())
            {
                Refresh();
            }
        }

        private bool UpdateDimensions()
        {
            if (Children.Any(n => n.Size == null))
                return false;

            (var nodesMinX, var nodesMaxX, var nodesMinY, var nodesMaxY) = Children.GetBounds();
            Size = new Size(nodesMaxX - nodesMinX + Padding * 2, nodesMaxY - nodesMinY + Padding * 2);
            Position = new Point(nodesMinX - Padding, nodesMinY - Padding);
            return true;
        }
    }
}
