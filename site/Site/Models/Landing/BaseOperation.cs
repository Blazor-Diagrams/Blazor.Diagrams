using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;

namespace Site.Models.Landing;

public abstract class BaseOperation : NodeModel
{
    private double _value;

    public event Action<BaseOperation>? ValueChanged;

    protected BaseOperation(Point position) : base(position)
    {
            
    }

    public double Value
    {
        get => _value;
        set
        {
            if (_value == value)
                return;
            
            _value = value;
            ValueChanged?.Invoke(this);
        }
    }
}