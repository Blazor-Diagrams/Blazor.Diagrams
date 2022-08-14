using Blazor.Diagrams;
using Blazor.Diagrams.Components;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Models;

namespace SharedDemo.Demos.Nodes
{
    public partial class SvgDemo
    {
        private Diagram _diagram = new Diagram();

        protected override void OnInitialized()
        {
            base.OnInitialized();

            LayoutData.Title = "SVG Nodes";
            LayoutData.Info = "You can also have SVG nodes! All you need to do is to set the Layer to RenderLayer.SVG.";
            LayoutData.DataChanged();

            InitializeDiagram();
        }

        private void InitializeDiagram()
        {
            _diagram.RegisterModelComponent<NodeModel, NodeWidget>();
            _diagram.RegisterModelComponent<SvgNodeModel, SvgNodeWidget>();
            _diagram.RegisterModelComponent<SvgGroupModel, DefaultGroupWidget>();

            var node1 = NewNode(50, 50);
            var node2 = NewNode(250, 250);
            var node3 = NewNode(500, 100);
            var node4 = NewNode(700, 350);
            _diagram.Nodes.Add(new[] { node1, node2, node3, node4 });

            _diagram.Links.Add(new LinkModel(node1.GetPort(PortAlignment.Right), node2.GetPort(PortAlignment.Left)));
            _diagram.Links.Add(new LinkModel(node2.GetPort(PortAlignment.Right), node3.GetPort(PortAlignment.Left)));
            _diagram.Links.Add(new LinkModel(node1.GetPort(PortAlignment.Right), node3.GetPort(PortAlignment.Left)));

            var group1 = _diagram.AddGroup(new SvgGroupModel(new[] { node1, node2 }));
            var group2 = _diagram.AddGroup(new SvgGroupModel(new[] { group1, node3 }));

            _diagram.Links.Add(new LinkModel(group2, node4));
        }

        private NodeModel NewNode(double x, double y, bool svg = true)
        {
            var node = svg ? new SvgNodeModel(new Point(x, y)) : new NodeModel(new Point(x, y));
            node.AddPort(PortAlignment.Left);
            node.AddPort(PortAlignment.Right);
            return node;
        }
    }
}
