using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core;
using System;
using System.Collections.Generic;
using System.Text;
using Blazor.Diagrams.Core.Geometry;

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

            var node1 = new NodeModel(new Point(80, 80), shape: Shapes.Rectangle);
            var node2 = new RoundedNode(new Point(280, 150), shape: Shapes.Circle);
            _diagram.Nodes.Add(node1);
            _diagram.Nodes.Add(node2);
            _diagram.Links.Add(new LinkModel(node1, node2)
            {
                SourceMarker = LinkMarker.Arrow,
                TargetMarker = LinkMarker.Arrow
            });
        }
    }

    class RoundedNode : NodeModel
    {
        public RoundedNode(Point position = null, ShapeDefiner shape = null) : base(position, RenderLayer.HTML, shape) { }
    }
}
