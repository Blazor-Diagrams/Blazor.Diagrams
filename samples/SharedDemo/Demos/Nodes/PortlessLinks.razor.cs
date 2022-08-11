using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams;
using Blazor.Diagrams.Core.Anchors;

namespace SharedDemo.Demos.Nodes
{
    public partial class PortlessLinks
    {
        private Diagram _diagram = new Diagram();

        protected override void OnInitialized()
        {
            base.OnInitialized();

            LayoutData.Title = "Portless Links";
            LayoutData.Info = "Starting from 2.0, you can create links between nodes directly! " +
                "All you need to specify is the shape of your nodes in order to calculate the connection points.";
            LayoutData.DataChanged();

            InitializeDiagram();
        }

        private void InitializeDiagram()
        {
            _diagram.RegisterModelComponent<RoundedNode, RoundedNodeWidget>();

            var node1 = new NodeModel(new Point(80, 80));
            var node2 = new RoundedNode(new Point(280, 150));
            var node3 = new NodeModel(new Point(400, 300));
            node3.AddPort(PortAlignment.Left);
            _diagram.Nodes.Add(node1);
            _diagram.Nodes.Add(node2);
            _diagram.Nodes.Add(node3);
            _diagram.Links.Add(new LinkModel(node1, node2)
            {
                SourceMarker = LinkMarker.Arrow,
                TargetMarker = LinkMarker.Arrow,
                Segmentable = true
            });
            _diagram.Links.Add(new LinkModel(new ShapeIntersectionAnchor(node2), new SinglePortAnchor(node3.GetPort(PortAlignment.Left)))
            {
                SourceMarker = LinkMarker.Arrow,
                TargetMarker = LinkMarker.Arrow,
                Segmentable = true
            });
        }
    }

    class RoundedNode : NodeModel
    {
        public RoundedNode(Point position = null) : base(position) { }

        public override IShape GetShape()
        {
            return Shapes.Circle(this);
        }
    }
}
