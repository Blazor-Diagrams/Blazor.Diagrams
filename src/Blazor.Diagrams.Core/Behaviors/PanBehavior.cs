using Blazor.Diagrams.Core.Models.Base;
using Blazor.Diagrams.Core.Models.Core;
using Microsoft.AspNetCore.Components.Web;
using System;

namespace Blazor.Diagrams.Core.Behaviors
{
    public class PanBehavior : Behavior
    {
        private Point? _initialPan;
        private double _lastClientX;
        private double _lastClientY;

        public PanBehavior(Diagram diagram) : base(diagram)
        {
            Diagram.MouseDown += Diagram_MouseDown;
            Diagram.MouseMove += Diagram_MouseMove;
            Diagram.MouseUp += Diagram_MouseUp;
        }

        private void Diagram_MouseDown(Model model, MouseEventArgs e)
        {
            if (!Diagram.Options.AllowPanning || model != null || e.ShiftKey || e.Button != (int)MouseEventButton.Left)
                return;

            _initialPan = Diagram.Pan;
            _lastClientX = e.ClientX;
            _lastClientY = e.ClientY;
        }

        private void Diagram_MouseMove(Model model, MouseEventArgs e)
        {
            if (!Diagram.Options.AllowPanning || _initialPan == null)
                return;

            var deltaX = e.ClientX - _lastClientX - (Diagram.Pan.X - _initialPan.X);
            var deltaY = e.ClientY - _lastClientY - (Diagram.Pan.Y - _initialPan.Y);
            Diagram.UpdatePan(deltaX, deltaY);
        }

        private void Diagram_MouseUp(Model model, MouseEventArgs e)
        {
            if (!Diagram.Options.AllowPanning)
                return;

            _initialPan = null;
        }

        public override void Dispose()
        {
            Diagram.MouseDown -= Diagram_MouseDown;
            Diagram.MouseMove -= Diagram_MouseMove;
            Diagram.MouseUp -= Diagram_MouseUp;
        }
    }
}
