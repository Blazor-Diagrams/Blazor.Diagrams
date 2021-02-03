using Blazor.Diagrams.Core.Models.Base;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Blazor.Diagrams.Core
{
    public class Layer<T> : IReadOnlyList<T> where T : Model
    {
        private readonly List<T> _items = new List<T>();

        public event Action<T[]>? Added;
        public event Action<T[]>? Removed;

        public void Add(params T[] items)
        {
            _items.AddRange(items);
            Added?.Invoke(items);
        }

        public void Remove(params T[] items)
        {
            foreach (var item in items)
            {
                _items.Remove(item);
            }

            // May contain items not removed (because they weren't in the list)
            // It's up to the user to only give existing items
            Removed?.Invoke(items);
        }

        public bool Contains(T item) => _items.Contains(item);

        public void Clear()
        {
            var items = _items.ToArray();
            _items.Clear();
            Removed?.Invoke(items);
        }

        public int Count => _items.Count;
        public T this[int index] => _items[index];
        public IEnumerator<T> GetEnumerator() => _items.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => _items.GetEnumerator();
    }
}
