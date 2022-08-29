using System;
using System.Collections.Generic;
using Blazor.Diagrams.Core.Models.Base;

namespace Blazor.Diagrams.Core.Controls;

public class ControlsLayer
{
    private readonly Dictionary<Model, ControlsContainer> _containers;

    public event Action<Model>? Changed;

    public ControlsLayer()
    {
        _containers = new Dictionary<Model, ControlsContainer>();
    }

    public IReadOnlyCollection<Model> Models => _containers.Keys;

    public ControlsContainer AddFor(Model model, ControlsType type = ControlsType.OnSelection)
    {
        if (_containers.ContainsKey(model))
            return _containers[model];
        
        var container = new ControlsContainer(model, type);
        container.Changed += OnChanged;
        model.Changed += OnChanged;
        _containers.Add(model, container);
        return container;
    }

    public ControlsContainer? GetFor(Model model)
    {
        return _containers.TryGetValue(model, out var container) ? container : null;
    }

    public bool RemoveFor(Model model)
    {
        if (!_containers.TryGetValue(model, out var container))
            return false;
        
        container.Changed -= OnChanged;
        model.Changed -= OnChanged;
        _containers.Remove(model);
        Changed?.Invoke(model);
        return true;
    }

    private void OnChanged(Model cause) => Changed?.Invoke(cause);
}