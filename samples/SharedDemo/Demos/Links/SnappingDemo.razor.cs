using Blazor.Diagrams;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;

namespace SharedDemo.Demos.Links
{
    public partial class SnappingDemo
    {
        private Diagram _diagram = new Diagram();

        protected override void OnInitialized()
        {
            base.OnInitialized();

            LayoutData.Title = "Link Snapping";
            LayoutData.Info = "While dragging a new link, it will try to find (and link) to the closest target within a radius.";
            LayoutData.DataChanged();

            InitializeDiagram();
        }

        private void InitializeDiagram()
        {
            _diagram.Options.Links.EnableSnapping = true;
            _diagram.Nodes.Add(new[] { NewNode(50, 80), NewNode(200, 350), NewNode(400, 100) });
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
