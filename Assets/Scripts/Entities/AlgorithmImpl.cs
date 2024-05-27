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
        private int _capacity;
        private int _count;

        public AlgorithmImpl(int capacity)
        {
            if (capacity < 0 || capacity > MaxCapacity)
            {
                throw new ArgumentOutOfRangeException();
            }
            _capacity = capacity;
            _count = 0;
            _elements = new ICommand?[MaxCapacity];
        }

        public ICommand this[int index]
        {
            get
            {
                if (index < 0 || index >= _count)
                {
                    throw new IndexOutOfRangeException();
                }
                return _elements[index]!;
            }

            set
            {
                if (index < 0 || index > _count)
                {
                    throw new IndexOutOfRangeException();
                }
                _elements[index] = value;
                if (index == _count)
                {
                    _count++;
                }
            }
        }

        public int Count => _count;

        public bool IsReadOnly => false;

        public int Capacity
        {
            get => _capacity;
            set
            {
                if (value < 0 || value > MaxCapacity)
                {
                    throw new ArgumentOutOfRangeException();
                }
                _capacity = value;
                if (_capacity < _count)
                {
                    for (int i = _capacity; i < _count; i++)
                    {
                        _elements[i] = null;
                    }
                    _count = _capacity;
                }
            }
        }

        public void Add(ICommand item)
        {
            if (_count == _capacity)
            {
                throw new Exception();
            }
            _elements[_count++] = item;
        }

        public void Insert(int index, ICommand item)
        {
            if (_count == _capacity)
            {
                throw new Exception();
            }
            for (int i = ++_count; i > index; i--)
            {
                _elements[i] = _elements[i - 1];
            }
            _elements[index] = item;
        }

        public bool Contains(ICommand item) => IndexOf(item) != -1;

        public int IndexOf(ICommand item)
        {
            for (int i = 0; i < _count; i++)
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
            if (index < 0 || index >= _count)
            {
                throw new ArgumentOutOfRangeException();
            }
            for (int i = index; i < _count - 1; i++)
            {
                _elements[i] = _elements[i + 1];
            }
            _count--;
        }

        public bool CopyTo(IAlgorithm algorithm)
        {
            if (algorithm is AlgorithmImpl al)
            {
                _elements.CopyTo(al._elements, 0);
                al._count = _count;
            }
            else
            {
                algorithm.Clear();
                for (int i = 0; i < _count; i++)
                {
                    algorithm.Add(_elements[i]!);
                }
            }
            return true;
        }

        public void CopyTo(ICommand[] array, int arrayIndex)
        {
            if (arrayIndex + _count > array.Length)
            {
                throw new IndexOutOfRangeException();
            }
            for (int i = 0; i < _count; i++)
            {
                array[arrayIndex + i] = _elements[i]!;
            }
        }

        public void Clear()
        {
            for (int i = 0; i < _count; i++)
            {
                _elements[i] = null;
            }
            _count = 0;
        }

        public IEnumerator<ICommand> GetEnumerator()
        {
            for (int i = 0; i < _count; i++)
            {
                yield return _elements[i]!;
            }
            yield break;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
