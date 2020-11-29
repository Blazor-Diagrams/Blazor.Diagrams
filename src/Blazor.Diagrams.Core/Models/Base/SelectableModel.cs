namespace Blazor.Diagrams.Core.Models.Base
{
    public abstract class SelectableModel : Model
    {
        public SelectableModel() { }

        public SelectableModel(string id) : base(id) { }

        public bool Selected { get; internal set; }
    }
}
