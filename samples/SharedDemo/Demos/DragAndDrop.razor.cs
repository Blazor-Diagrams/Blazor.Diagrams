using Blazor.Diagrams.Core;
using Blazor.Diagrams.Core.Models;
using Microsoft.AspNetCore.Components.Web;

namespace SharedDemo.Demos
{
    public partial class DragAndDrop
    {
        private readonly DiagramManager _diagramManager = new DiagramManager();
        private int? _draggedType;

        protected override void OnInitialized()
        {
            base.OnInitialized();

            LayoutData.Title = "Drag & Drop";
            LayoutData.Info = "A very simple drag & drop implementation using the HTML5 events.";
            LayoutData.DataChanged();

            _diagramManager.RegisterModelComponent<BotAnswerNode, BotAnswerWidget>();
        }

        private void OnDragStart(int key)
        {
            // Can also use transferData, but this is probably "faster"
            _draggedType = key;
        }

        private void OnDrop(DragEventArgs e)
        {
            if (_draggedType == null) // Unkown item
                return;

            var position = _diagramManager.GetRelativePoint(e.ClientX, e.ClientY);
            var node = _draggedType == 0 ? new NodeModel(position) : new BotAnswerNode(position);
            node.AddPort(PortAlignment.Top);
            node.AddPort(PortAlignment.Bottom);
            _diagramManager.Nodes.Add(node);
            _draggedType = null;
        }
    }
}
