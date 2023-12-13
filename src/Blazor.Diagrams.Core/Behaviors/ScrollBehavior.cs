using Blazor.Diagrams.Core.Behaviors.Base;
using Blazor.Diagrams.Core.Events;
using Blazor.Diagrams.Core.Options;

namespace Blazor.Diagrams.Core.Behaviors
{
	public class ScrollBehavior : WheelBehavior
	{
		public ScrollBehavior(Diagram diagram)
			: base(diagram)
		{
		}

		protected override void OnDiagramWheel(WheelEventArgs e)
		{
			if (Diagram.Container == null || !IsBehaviorEnabled(e))
				return;

			var x = Diagram.Pan.X - (e.DeltaX / Diagram.Options.Zoom.ScaleFactor);
			var y = Diagram.Pan.Y - (e.DeltaY / Diagram.Options.Zoom.ScaleFactor);

			Diagram.SetPan(x, y);
		}
	}
}
