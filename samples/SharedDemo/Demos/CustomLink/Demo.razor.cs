using Blazor.Diagrams.Core;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Core;

namespace SharedDemo.Demos.CustomLink
{
    partial class Demo
    {
        private readonly Diagram _diagram = new Diagram();

        protected override void OnInitialized()
        {
            base.OnInitialized();

            LayoutData.Title = "Custom link";
            LayoutData.Info = "Creating your own custom links is very easy!";
            LayoutData.DataChanged();

            _diagram.RegisterModelComponent<ThickLink, ThickLinkWidget>();
            // Also usable: _diagram.Options.Links.DefaultLinkComponent = typeof(ThickLink);

            var node1 = NewNode(50, 50);
            var node2 = NewNode(300, 300);
            var node3 = NewNode(500, 50);

            _diagram.Nodes.Add(node1, node2, node3);
            _diagram.Links.Add(new ThickLink(node1.GetPort(PortAlignment.Right), node2.GetPort(PortAlignment.Left)));
            _diagram.Links.Add(new ThickLink(node2.GetPort(PortAlignment.Right), node3.GetPort(PortAlignment.Left)));
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
