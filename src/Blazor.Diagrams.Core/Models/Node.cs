using System.Collections.Generic;

namespace Blazor.Diagrams.Core.Models
{
    public class Node : Model
    {
        public Node() { }

        public Node(string id) : base(id) { }

        public Point Offset { get; set; }
        public Point Position { get; set; }
        public List<Port> Ports { get; } = new List<Port>();
    }
}
