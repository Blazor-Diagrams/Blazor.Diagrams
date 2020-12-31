using Blazor.Diagrams.Core;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Core;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Diagnostics;

namespace Blazor.Diagrams.Components.Groups
{
    public partial class GroupContainer : IDisposable
    {
        private readonly Stopwatch _stopwatch = new Stopwatch();
        private int _totalRenders = 0;

        [Parameter]
        public GroupModel Group { get; set; }

        [Parameter]
        public ushort Padding { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [CascadingParameter(Name = "DiagramManager")]
        public DiagramManager DiagramManager { get; set; }

        public double X => Group.Position.X;
        public double Y => Group.Position.Y;
        public double Width => Group.Size.Width;
        public double Height => Group.Size.Height;

        public void Dispose()
        {
            Group.Changed -= OnGroupChanged;
        }

        protected override void OnParametersSet()
        {
            Group.Changed += OnGroupChanged;
        }

        protected override bool ShouldRender()
        {
            _stopwatch.Start();
            return true;
        }

        protected override void OnAfterRender(bool firstRender)
        {
            base.OnAfterRender(firstRender);
            _totalRenders++;
            Console.WriteLine($"Group: {Group.Id}, Render: #{_totalRenders}, Time: {_stopwatch.Elapsed.TotalMilliseconds}ms");
            _stopwatch.Reset();
        }

        private void OnGroupChanged()
        {
            StateHasChanged(); // Todo: Use _shouldRender pattern
        }

        private void OnMouseDown(MouseEventArgs e) => DiagramManager.OnMouseDown(Group, e);

        private void OnMouseUp(MouseEventArgs e) => DiagramManager.OnMouseUp(Group, e);
    }
}
