using Blazor.Diagrams.Core.Events;
using Blazor.Diagrams.Core.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blazor.Diagrams.Core.Behaviors;

public class KeyboardShortcutsBehavior : Behavior
{
    private readonly Dictionary<string, Func<Diagram, ValueTask>> _shortcuts;

    public KeyboardShortcutsBehavior(Diagram diagram) : base(diagram)
    {
        _shortcuts = new Dictionary<string, Func<Diagram, ValueTask>>();
        SetShortcut("Delete", false, false, false, KeyboardShortcutsDefaults.DeleteSelection);
        SetShortcut("g", true, false, true, KeyboardShortcutsDefaults.Grouping);

        Diagram.KeyDown += OnDiagramKeyDown;
    }

    public void SetShortcut(string key, bool ctrl, bool shift, bool alt, Func<Diagram, ValueTask> action)
    {
        var k = KeysUtils.GetStringRepresentation(ctrl, shift, alt, key);
        _shortcuts[k] = action;
    }

    public bool RemoveShortcut(string key, bool ctrl, bool shift, bool alt)
    {
        var k = KeysUtils.GetStringRepresentation(ctrl, shift, alt, key);
        return _shortcuts.Remove(k);
    }

    private async void OnDiagramKeyDown(KeyboardEventArgs e)
    {
        var k = KeysUtils.GetStringRepresentation(e.CtrlKey, e.ShiftKey, e.AltKey, e.Key);
        if (_shortcuts.TryGetValue(k, out var action))
        {
            await action(Diagram);
        }
    }

    public override void Dispose()
    {
        Diagram.KeyDown -= OnDiagramKeyDown;
    }
}
