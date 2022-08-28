using Blazor.Diagrams.Core.Events;
using Blazor.Diagrams.Core.Models.Base;

namespace Blazor.Diagrams.Core.UserActions;

public class UserActionsBehavior : Behavior
{
    public UserActionsBehavior(Diagram diagram) : base(diagram)
    {
        Diagram.PointerEnter += OnPointerEnter;
        Diagram.PointerLeave += OnPointerLeave;
        Diagram.SelectionChanged += OnSelectionChanged;
    }

    private void OnSelectionChanged(SelectableModel model)
    {
        var userActions = Diagram.UserActions.GetFor(model);
        if (userActions is not { Type: UserActionsType.OnSelection })
            return;

        if (model.Selected)
        {
            userActions.Show();
        }
        else
        {
            userActions.Hide();
        }
    }

    private void OnPointerEnter(Model? model, PointerEventArgs e)
    {
        if (model == null)
            return;
        
        var userActions = Diagram.UserActions.GetFor(model);
        if (userActions is not { Type: UserActionsType.OnHover })
            return;
        
        userActions.Show();
    }

    private void OnPointerLeave(Model? model, PointerEventArgs e)
    {
        if (model == null)
            return;
        
        var userActions = Diagram.UserActions.GetFor(model);
        if (userActions is not { Type: UserActionsType.OnHover })
            return;
        
        userActions.Hide();
    }

    public override void Dispose()
    {
        Diagram.PointerEnter -= OnPointerEnter;
        Diagram.PointerLeave -= OnPointerLeave;
        Diagram.SelectionChanged -= OnSelectionChanged;
    }
}