#nullable enable


namespace CodingStrategy.Entities
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class AlgorithmImpl : IAlgorithm
    {
        private const int MaxCapacity = 10;

        private readonly ICommand?[] _elements;
        private int _bound;

        public AlgorithmImpl()
        {
            _elements = new ICommand?[MaxCapacity];
            _bound = 0;
        }

        public ICommand this[int index]
        {
            get
            {
                if (index < 0 || index >= _bound)
                {
                    throw new IndexOutOfRangeException();
                }
                return _elements[index]!;
            }

            set
            {
                if (index < 0 || index >= _bound)
                {
                    throw new IndexOutOfRangeException();
                }
                _elements[index] = value;
            }
        }

        public int Count => _bound;

        public bool IsReadOnly => false;

        public void Add(ICommand item)
        {
            if (_bound == _elements.Length)
            {
                throw new Exception();
            }
            _elements[_bound++] = item;
        }

        public void Insert(int index, ICommand item)
        {
            if (_bound == _elements.Length)
            {
                throw new Exception();
            }
            for (int i = ++_bound; i > index; i--)
            {
                _elements[i] = _elements[i - 1];
            }
            _elements[index] = item;
        }

        public bool Contains(ICommand item) => IndexOf(item) != -1;

        public int IndexOf(ICommand item)
        {
            for (int i = 0; i < _bound; i++)
            {
                if (item.Equals(_elements[i]))
                {
                    return i;
                }
            }
            return -1;
        }

        public bool Remove(ICommand item)
        {
            int index = IndexOf(item);
            if (index < 0)
            {
                return false;
            }
            RemoveAt(index);
            return true;
        }

        public void RemoveAt(int index)
        {
            for (int i = index; i < _bound - 1; i++)
            {
                _elements[i] = _elements[i + 1];
            }
        }

        public bool CopyTo(IAlgorithm algorithm)
        {
            if (algorithm is AlgorithmImpl al)
            {
                _elements.CopyTo(al._elements, 0);
                al._bound = _bound;
            }
            else
            {
                algorithm.Clear();
                for (int i = 0; i < _bound; i++)
                {
                    algorithm.Add(_elements[i]!);
                }
            }
            return true;
        }

        public void CopyTo(ICommand[] array, int arrayIndex)
        {
            if (arrayIndex + _bound > array.Length)
            {
                throw new IndexOutOfRangeException();
            }
            for (int i = 0; i < _bound; i++)
            {
                array[arrayIndex + i] = _elements[i]!;
            }
        }

        public void Clear()
        {
            for (int i = 0; i < _bound; i++)
            {
                _elements[i] = null;
            }
            _bound = 0;
        }

        public IEnumerator<ICommand> GetEnumerator()
        {
            for (int i = 0; i < _bound; i++)
            {
                yield return _elements[i]!;
            }
            yield break;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
