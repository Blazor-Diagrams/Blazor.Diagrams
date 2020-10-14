using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Core;

namespace SharedDemo.Demos
{
    public class BotAnswerNode : NodeModel
    {
        public BotAnswerNode(Point position = null) : base(position) { }

        public string Answer { get; set; }
    }
}
