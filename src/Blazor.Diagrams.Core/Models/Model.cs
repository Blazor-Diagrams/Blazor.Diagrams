using System;

namespace Blazor.Diagrams.Core.Models
{
    public abstract class Model
    {
        public Model() : this(Guid.NewGuid().ToString()) { }

        public Model(string id)
        {
            Id = id;
        }

        public event Action Changed;

        public string Id { get; }

        public virtual void Refresh() => Changed?.Invoke();
    }
}
