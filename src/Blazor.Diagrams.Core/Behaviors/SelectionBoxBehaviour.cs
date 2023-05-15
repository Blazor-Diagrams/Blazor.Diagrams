using Blazor.Diagrams.Core.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazor.Diagrams.Core.Behaviors
{
    public class SelectionBoxBehavior : Behavior
    {
        public SelectionBoxBehavior(Diagram diagram)
            : base(diagram)
        {
        }

        public event EventHandler<Rectangle?> BoundsChanged;

        public override void Dispose()
        {
        }
    }
}
