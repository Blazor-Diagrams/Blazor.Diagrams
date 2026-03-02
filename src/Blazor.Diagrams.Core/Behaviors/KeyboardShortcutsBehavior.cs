using Blazor.Diagrams.Core.Events;
using Blazor.Diagrams.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazor.Diagrams.Core.Behaviors;

public class KeyboardShortcutsBehavior : Behavior
{
    public record KeyboardShortcut(string Key, bool Ctrl, bool Shift, bool Alt, Func<Diagram, ValueTask> Action);
    
    private readonly Dictionary<string, KeyboardShortcut> _shortcuts;

    public KeyboardShortcutsBehavior(Diagram diagram) : base(diagram)
    {
        _shortcuts = new Dictionary<string, KeyboardShortcut>();
        SetShortcut("Delete", false, false, false, KeyboardShortcutsDefaults.DeleteSelection);
        SetShortcut("g", true, false, true, KeyboardShortcutsDefaults.Grouping);

        Diagram.KeyDown += OnDiagramKeyDown;
    }

    public KeyboardShortcut[] GetShortcuts()
    {
        return _shortcuts.Values.ToArray();
    }
    
    public void SetShortcut(string key, bool ctrl, bool shift, bool alt, Func<Diagram, ValueTask> action)
    {
        var k = KeysUtils.GetStringRepresentation(ctrl, shift, alt, key);
        _shortcuts[k] = new KeyboardShortcut(key, ctrl, shift, alt, action);
    }
    
    public bool RemoveShortcut(string key, bool ctrl, bool shift, bool alt)
    {
        var k = KeysUtils.GetStringRepresentation(ctrl, shift, alt, key);
        return _shortcuts.Remove(k);
    }

    public void RemoveAllShortcuts()
    {
        _shortcuts.Clear();
    }

    private async void OnDiagramKeyDown(KeyboardEventArgs e)
    {
        var k = KeysUtils.GetStringRepresentation(e.CtrlKey, e.ShiftKey, e.AltKey, e.Key);
        if (_shortcuts.TryGetValue(k, out var shortcut))
        {
            await shortcut.Action(Diagram);
        }
    }

    public override void Dispose()
    {
        Diagram.KeyDown -= OnDiagramKeyDown;
    }
}
