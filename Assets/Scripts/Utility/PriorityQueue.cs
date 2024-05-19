#nullable enable


using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace CodingStrategy.Utility
{
    public class PriorityQueue<T> : IReadOnlyCollection<T>
    {
        private readonly IComparer<T> _comparer;
        private readonly IList<T> _items;

        public PriorityQueue(IComparer<T> comparer)
        {
            _comparer = comparer;
            _items = new List<T>();
        }

        public int Count => _items.Count;

        public void Enqueue(T item)
        {
            _items.Add(item);
            SinkUp(Count - 1);
        }

        public T Dequeue()
        {
            if (!TryDequeue(out T value))
            {
                throw new Exception();
            }

            return value;
        }

        public bool TryDequeue([MaybeNullWhen(false)] out T value)
        {
            if (_items.Count == 0)
            {
                value = default!;
                return false;
            }

            int lastIndex = Count - 1;

            (_items[0], _items[lastIndex]) = (_items[lastIndex], _items[0]);
            value = _items[lastIndex];
            _items.RemoveAt(lastIndex);

            SinkDown(0);
            return true;
        }

        public void Clear()
        {
            _items.Clear();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private int Compare(T x, T y)
        {
            return _comparer.Compare(x, y);
        }

        private void SinkUp(int index)
        {
            if (index == 0)
            {
                return;
            }

            int parent = (index - 1) / 2;

            while (parent != index)
            {
                T currentItem = _items[index];
                T parentItem = _items[parent];
                int compare = Compare(currentItem, parentItem);

                if (compare >= 0)
                {
                    break;
                }

                (_items[index], _items[parent]) = (_items[parent], _items[index]);

                index = parent;
                parent = (index - 1) / 2;
            }
        }

        private void SinkDown(int index)
        {
            int bound = Math.Max(index, _items.Count);

            if (index == bound)
            {
                return;
            }

            while (index != bound)
            {
                int child = index * 2 + 1;

                if (child >= bound)
                {
                    break;
                }

                T currentItem = _items[index];
                T childItem = _items[child];

                if (child + 1 < bound)
                {
                    T nextChildItem = _items[child + 1];
                    if (Compare(childItem, nextChildItem) > 0)
                    {
                        child += 1;
                        childItem = nextChildItem;
                    }
                }

                int compare = Compare(currentItem, childItem);

                if (compare <= 0)
                {
                    break;
                }

                index = child;
            }
        }
    }
}
