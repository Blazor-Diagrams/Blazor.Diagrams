using Blazor.Diagrams.Core.Events;
using Blazor.Diagrams.Core.Models.Base;

namespace Blazor.Diagrams.Core.Controls;

public class ControlsBehavior : Behavior
{
	public ControlsBehavior(Diagram diagram) : base(diagram)
	{
		Diagram.PointerEnter += OnPointerEnter;
		Diagram.PointerLeave += OnPointerLeave;
		Diagram.SelectionChanged += OnSelectionChanged;
	}

	private void OnSelectionChanged(SelectableModel model)
	{
		var controls = Diagram.Controls.GetFor(model, ControlsType.OnSelection);
		if (controls is null)
			return;

		if (model.Selected)
		{
			controls.Show();
		}
		else
		{
			controls.Hide();
		}
	}

	private void OnPointerEnter(Model? model, PointerEventArgs e)
	{
		if (model == null)
			return;

		Diagram.Controls.GetFor(model, ControlsType.OnHover)?.Show();
	}

	private void OnPointerLeave(Model? model, PointerEventArgs e)
	{
		if (model == null)
			return;

		Diagram.Controls.GetFor(model, ControlsType.OnHover)?.Hide();
	}

	public override void Dispose()
	{
		Diagram.PointerEnter -= OnPointerEnter;
		Diagram.PointerLeave -= OnPointerLeave;
		Diagram.SelectionChanged -= OnSelectionChanged;
	}
}