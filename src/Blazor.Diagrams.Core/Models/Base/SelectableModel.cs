using System;

namespace Blazor.Diagrams.Core.Models.Base
{
    public abstract class SelectableModel : Model
    {
        public SelectableModel() { }

        public SelectableModel(string id) : base(id) { }

        private bool _selected;
        public bool Selected {
	        get
	        {
		        return _selected;
	        }
	        internal set
	        {
				  _selected = value;

				  SelectedChanged?.Invoke(this, this);
	        }
        }

        public EventHandler<SelectableModel> SelectedChanged;
    }
}
