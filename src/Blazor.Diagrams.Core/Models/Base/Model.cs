using System;

namespace Blazor.Diagrams.Core.Models.Base
{
    public abstract class Model
    {
        public Model() : this(Guid.NewGuid().ToString()) { }

        public Model(string id)
        {
            Id = id;
        }

        public event Action<Model>? Changed;

        public string Id { get; }
        public bool Locked { get; set; }

        public virtual void Refresh() => Changed?.Invoke(this);
    }
}
