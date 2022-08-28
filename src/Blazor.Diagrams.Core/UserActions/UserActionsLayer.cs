using System;
using System.Collections.Generic;
using Blazor.Diagrams.Core.Models.Base;

namespace Blazor.Diagrams.Core.UserActions;

public class UserActionsLayer
{
    private readonly Dictionary<Model, UserActionsContainer> _containers;

    public event Action<Model>? Changed;

    public UserActionsLayer()
    {
        _containers = new Dictionary<Model, UserActionsContainer>();
    }

    public IReadOnlyCollection<Model> Models => _containers.Keys;

    public UserActionsContainer AddFor(Model model, UserActionsType type = UserActionsType.OnSelection)
    {
        if (_containers.ContainsKey(model))
            return _containers[model];
        
        var container = new UserActionsContainer(model, type);
        container.Changed += OnChanged;
        model.Changed += OnChanged;
        _containers.Add(model, container);
        return container;
    }

    public UserActionsContainer? GetFor(Model model)
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