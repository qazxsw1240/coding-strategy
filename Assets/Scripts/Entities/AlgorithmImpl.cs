#nullable enable


using CodingStrategy.Entities.Runtime.CommandImpl;

namespace CodingStrategy.Entities
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class AlgorithmImpl : IAlgorithm
    {
        private const int MaxCapacity = 10;

        private static readonly ICommand DefaultCommand = new EmptyCommand();

        private readonly ICommand?[] _elements;
        private int _capacity;

        public AlgorithmImpl(int capacity)
        {
            if (capacity < 0 || capacity > MaxCapacity)
            {
                throw new ArgumentOutOfRangeException();
            }

            _capacity = capacity;
            _elements = new ICommand?[MaxCapacity];
            for (int i = 0; i < MaxCapacity; i++)
            {
                _elements[i] = DefaultCommand;
            }
        }

        public ICommand this[int index]
        {
            get
            {
                if (index < 0 || index >= _capacity)
                {
                    throw new IndexOutOfRangeException();
                }

                return _elements[index]!;
            }

            set
            {
                if (index < 0 || index > _capacity)
                {
                    throw new IndexOutOfRangeException();
                }

                _elements[index] = value;
            }
        }

        public int Count => _capacity;

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

                int previousCapacity = _capacity;
                _capacity = value;
                if (_capacity < previousCapacity)
                {
                    for (int i = _capacity; i < previousCapacity; i++)
                    {
                        _elements[i] = DefaultCommand;
                    }
                }
            }
        }

        public void Add(ICommand item)
        {
            //
        }

        public void Insert(int index, ICommand item)
        {
            for (int i = _capacity; i > index; i--)
            {
                _elements[i] = _elements[i - 1];
            }

            _elements[index] = item;
        }

        public bool Contains(ICommand item) => IndexOf(item) != -1;

        public int IndexOf(ICommand item)
        {
            for (int i = 0; i < _capacity; i++)
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
            if (index < 0 || index >= _capacity)
            {
                throw new ArgumentOutOfRangeException();
            }

            for (int i = index; i < _capacity - 1; i++)
            {
                _elements[i] = _elements[i + 1];
            }

            _elements[_capacity] = DefaultCommand;
        }

        public bool CopyTo(IAlgorithm algorithm)
        {
            if (algorithm is AlgorithmImpl al)
            {
                _elements.CopyTo(al._elements, 0);
                al._capacity = _capacity;
            }
            else
            {
                algorithm.Clear();
                for (int i = 0; i < _capacity; i++)
                {
                    algorithm.Add(_elements[i]!);
                }
            }

            return true;
        }

        public ICommand[] AsArray()
        {
            ICommand[] commands = new ICommand[_capacity];

            for (int i = 0; i < _capacity; i++)
            {
                commands[i] = _elements[i]!;
            }

            return commands;
        }

        public void CopyTo(ICommand[] array, int arrayIndex)
        {
            if (arrayIndex + _capacity > array.Length)
            {
                throw new IndexOutOfRangeException();
            }

            for (int i = 0; i < _capacity; i++)
            {
                array[arrayIndex + i] = _elements[i]!;
            }
        }

        public void Clear()
        {
            for (int i = 0; i < _capacity; i++)
            {
                _elements[i] = null;
            }
        }

        public IEnumerator<ICommand> GetEnumerator()
        {
            for (int i = 0; i < _capacity; i++)
            {
                yield return _elements[i]!;
            }

            yield break;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
