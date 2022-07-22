using Blazor.Diagrams;
using Blazor.Diagrams.Core;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;

namespace SharedDemo.Demos.Links
{
    public partial class PathGeneratorsDemo
    {
        private Diagram _diagram = new Diagram();

        protected override void OnInitialized()
        {
            base.OnInitialized();

            LayoutData.Title = "Link Path Generators";
            LayoutData.Info = "Path generators are functions that take as input the calculated route and output SVG paths, " +
                "alongside the markers positions and their angles. There are currently two generators: Straight and Smooth.";
            LayoutData.DataChanged();

            InitializeDiagram();
        }

        private void InitializeDiagram()
        {
            var node1 = NewNode(50, 80);
            var node2 = NewNode(300, 350);
            var node3 = NewNode(400, 100);

            _diagram.Nodes.Add(new[] { node1, node2, node3 });

            var link1 = new LinkModel(node1.GetPort(PortAlignment.Right), node2.GetPort(PortAlignment.Left))
            {
                Router = Routers.Normal,
                PathGenerator = PathGenerators.Straight
            };
            link1.Labels.Add(new LinkLabelModel(link1, "Straight"));

            var link2 = new LinkModel(node2.GetPort(PortAlignment.Right), node3.GetPort(PortAlignment.Left))
            {
                Router = Routers.Normal,
                PathGenerator = PathGenerators.Smooth
            };
            link2.Labels.Add(new LinkLabelModel(link2, "Smooth"));

            _diagram.Links.Add(new[] { link1, link2 });
        }

        private NodeModel NewNode(double x, double y)
        {
            var node = new NodeModel(new Point(x, y));
            node.AddPort(PortAlignment.Bottom);
            node.AddPort(PortAlignment.Top);
            node.AddPort(PortAlignment.Left);
            node.AddPort(PortAlignment.Right);
            return node;
        }
    }
}
