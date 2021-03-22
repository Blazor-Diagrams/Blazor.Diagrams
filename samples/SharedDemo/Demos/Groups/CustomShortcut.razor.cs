using Blazor.Diagrams.Core;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Geometry;

namespace SharedDemo.Demos.Groups
{
    public partial class CustomShortcut
    {
        private Diagram _diagram = new Diagram();

        protected override void OnInitialized()
        {
            base.OnInitialized();

            LayoutData.Title = "Custom Shortcut";
            LayoutData.Info = "You can customize what needs to be pressed to group selected nodes. CTRL+SHIFT+K in this example.";
            LayoutData.DataChanged();

            _diagram.Options.Groups.Enabled = true;
            _diagram.Options.Groups.KeyboardShortcut = e => e.CtrlKey && e.ShiftKey && e.Key.ToLower() == "k";

            var node1 = NewNode(50, 50);
            var node2 = NewNode(250, 250);
            var node3 = NewNode(500, 100);
            _diagram.Nodes.Add(new[] { node1, node2, node3 });

            _diagram.Links.Add(new LinkModel(node1.GetPort(PortAlignment.Right), node2.GetPort(PortAlignment.Left)));
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
