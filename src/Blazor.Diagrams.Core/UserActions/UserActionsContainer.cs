using System;
using System.Collections;
using System.Collections.Generic;
using Blazor.Diagrams.Core.Models.Base;

namespace Blazor.Diagrams.Core.UserActions;

public class UserActionsContainer : IReadOnlyList<UserAction>
{
    private readonly List<UserAction> _actions = new(4);

    public event Action<Model>? Changed;

    public UserActionsContainer(Model model, UserActionsType type = UserActionsType.OnSelection)
    {
        Model = model;
        Type = type;
    }

    public Model Model { get; }
    public UserActionsType Type { get; set; }
    public bool Visible { get; private set; }

    public void Show()
    {
        if (Visible)
            return;
        
        Visible = true;
        Changed?.Invoke(Model);
    }

    public void Hide()
    {
        if (!Visible)
            return;
        
        Visible = false;
        Changed?.Invoke(Model);
    }

    public void Add(UserAction action)
    {
        _actions.Add(action);
        Changed?.Invoke(Model);
    }

    public void Remove(UserAction action)
    {
        if (_actions.Remove(action))
        {
            Changed?.Invoke(Model);
        }
    }

    public int Count => _actions.Count;
    public UserAction this[int index] => _actions[index];
    public IEnumerator<UserAction> GetEnumerator() => _actions.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => _actions.GetEnumerator();
}