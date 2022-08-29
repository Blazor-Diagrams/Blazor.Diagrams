using System;
using System.Collections;
using System.Collections.Generic;
using Blazor.Diagrams.Core.Models.Base;

namespace Blazor.Diagrams.Core.Controls;

public class ControlsContainer : IReadOnlyList<Control>
{
    private readonly List<Control> _actions = new(4);

    public event Action<Model>? Changed;

    public ControlsContainer(Model model, ControlsType type = ControlsType.OnSelection)
    {
        Model = model;
        Type = type;
    }

    public Model Model { get; }
    public ControlsType Type { get; set; }
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

    public ControlsContainer Add(Control action)
    {
        _actions.Add(action);
        Changed?.Invoke(Model);
        return this;
    }

    public ControlsContainer Remove(Control action)
    {
        if (_actions.Remove(action))
        {
            Changed?.Invoke(Model);
        }

        return this;
    }

    public int Count => _actions.Count;
    public Control this[int index] => _actions[index];
    public IEnumerator<Control> GetEnumerator() => _actions.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => _actions.GetEnumerator();
}