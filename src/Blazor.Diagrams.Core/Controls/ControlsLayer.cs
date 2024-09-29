using Blazor.Diagrams.Core.Models.Base;

namespace Blazor.Diagrams.Core.Controls;

public class ControlsLayer
{
	private readonly Dictionary<(Model Model, ControlsType Type), ControlsContainer> _containers = new();

	public event Action<Model>? ChangeCaused;

	public IEnumerable<Model> Models => _containers.Keys.Select(key => key.Model);
	public IEnumerable<(Model Model, ControlsType Type)> ContainersKeys => _containers.Keys;

	public ControlsContainer AddFor(Model model, ControlsType type = ControlsType.OnSelection)
	{
		var key = (model, type);
		if (_containers.TryGetValue(key, out ControlsContainer? container))
			return container;

		container = new(model, type);
		container.VisibilityChanged += OnVisibilityChanged;
		container.ControlsChanged += RefreshIfVisible;
		model.Changed += RefreshIfVisible;

		_containers.Add(key, container);

		return container;
	}

	public ControlsContainer? GetFor(Model model, ControlsType type)
	{
		if (_containers.TryGetValue((model, type), out ControlsContainer? container))
			return container;

		return null;
	}

	public bool RemoveFor(Model model, ControlsType type)
	{
		var key = (model, type);
		if (!_containers.TryGetValue(key, out var container))
			return false;

		container.VisibilityChanged -= OnVisibilityChanged;
		container.ControlsChanged -= RefreshIfVisible;
		model.Changed -= RefreshIfVisible;
		_containers.Remove(key);
		ChangeCaused?.Invoke(model);
		return true;
	}

	public bool RemoveFor(Model model)
	{
		bool removed = false;
		foreach (ControlsType type in (ControlsType[])Enum.GetValues(typeof(ControlsType)))
		{
			var key = (model, type);
			if (_containers.TryGetValue(key, out var container))
			{
				container.VisibilityChanged -= OnVisibilityChanged;
				container.ControlsChanged -= RefreshIfVisible;
				model.Changed -= RefreshIfVisible;
				_containers.Remove(key);
				ChangeCaused?.Invoke(model);
				removed = true;
			}

		}

		return removed;
	}

	public bool AreVisibleFor(Model model, ControlsType type) => GetFor(model, type)?.Visible ?? false;

	private void RefreshIfVisible(Model cause)
	{
		foreach (ControlsType type in (ControlsType[])Enum.GetValues(typeof(ControlsType)))
		{
			if (AreVisibleFor(cause, type))
			{
				ChangeCaused?.Invoke(cause);
				return;
			}
		}

	}

	private void OnVisibilityChanged(Model cause) => ChangeCaused?.Invoke(cause);
}