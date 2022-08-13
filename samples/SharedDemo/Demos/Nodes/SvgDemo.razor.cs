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

            var node1 = NewNode(80, 80);
            var node2 = NewNode(280, 150);
            _diagram.Nodes.Add(node1);
            _diagram.Nodes.Add(node2);
            _diagram.Links.Add(new LinkModel(node1.GetPort(PortAlignment.Right), node2.GetPort(PortAlignment.Left)));

            _diagram.AddGroup(new SvgGroupModel(new[] { node1, node2 }));
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
