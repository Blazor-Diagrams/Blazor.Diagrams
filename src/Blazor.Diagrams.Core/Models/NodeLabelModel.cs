using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models.Base;
using System;

namespace Blazor.Diagrams.Core.Models
{
    public class NodeLabelModel : MovableModel
    {
        public event Action<NodeLabelModel>? ContentChanged;
        public NodeLabelModel(NodeModel parent, string id, string content, Point? position =null) : base(id, position)
        {
            Parent = parent;
            Content = content;
        }

        public NodeLabelModel(NodeModel parent, string content, Point? position = null) :base(position)
        {
            Parent = parent;
            Content = content;
        }

        public NodeModel Parent { get; }
        string _Content;
        public string Content 
        { 
            get { return _Content; }
            set 
            { 
                if (_Content != value) 
                { 
                    _Content = value; 
                    Size = null;
                    ContentChanged?.Invoke(this);
                }
            }
        }
        public Alignment Align { get; set; } = Alignment.Center;
        public Size? Size { get; set; } = null;
        public override void SetPosition(double x, double y)
        {
            base.SetPosition(x, y);
            Refresh();
        }
    }

    [Flags]
    public enum Alignment
    {
        Center = 0,
        Top = 1,
        Right = 2,
        Bottom = 4,
        Left = 8
    }
}
