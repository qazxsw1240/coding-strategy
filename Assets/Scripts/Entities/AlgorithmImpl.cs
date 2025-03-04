#nullable enable

using System;
using System.Collections;
using System.Collections.Generic;

using CodingStrategy.Entities.Runtime.Command;

namespace CodingStrategy.Entities
{
    public class AlgorithmImpl : IAlgorithm
    {
        private const int MaxCapacity = 10;

        private static readonly ICommand DefaultCommand = new EmptyCommand();

        private readonly ICommand?[] _elements;

        public AlgorithmImpl(int capacity)
        {
            if (capacity < 0 || capacity > MaxCapacity)
            {
                throw new ArgumentOutOfRangeException();
            }

            Count = capacity;
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
                if (index < 0 || index >= Count)
                {
                    throw new IndexOutOfRangeException();
                }

                return _elements[index]!;
            }

            set
            {
                if (index < 0 || index > Count)
                {
                    throw new IndexOutOfRangeException();
                }

                _elements[index] = value;
            }
        }

        public int Count { get; private set; }

        public bool IsReadOnly => false;

        public int Capacity
        {
            get => Count;
            set
            {
                if (value < 0 || value > MaxCapacity)
                {
                    throw new ArgumentOutOfRangeException();
                }

                int previousCapacity = Count;
                Count = value;
                if (Count < previousCapacity)
                {
                    for (int i = Count; i < previousCapacity; i++)
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
            for (int i = Count; i > index; i--)
            {
                _elements[i] = _elements[i - 1];
            }

            _elements[index] = item;
        }

        public bool Contains(ICommand item)
        {
            return IndexOf(item) != -1;
        }

        public int IndexOf(ICommand item)
        {
            for (int i = 0; i < Count; i++)
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
            if (index < 0 || index >= Count)
            {
                throw new ArgumentOutOfRangeException();
            }

            for (int i = index; i < Count - 1; i++)
            {
                _elements[i] = _elements[i + 1];
            }

            _elements[Count] = DefaultCommand;
        }

        public bool CopyTo(IAlgorithm algorithm)
        {
            if (algorithm is AlgorithmImpl al)
            {
                _elements.CopyTo(al._elements, 0);
                al.Count = Count;
            }
            else
            {
                algorithm.Clear();
                for (int i = 0; i < Count; i++)
                {
                    algorithm.Add(_elements[i]!);
                }
            }

            return true;
        }

        public ICommand[] AsArray()
        {
            ICommand[] commands = new ICommand[Count];

            for (int i = 0; i < Count; i++)
            {
                commands[i] = _elements[i]!;
            }

            return commands;
        }

        public void CopyTo(ICommand[] array, int arrayIndex)
        {
            if (arrayIndex + Count > array.Length)
            {
                throw new IndexOutOfRangeException();
            }

            for (int i = 0; i < Count; i++)
            {
                array[arrayIndex + i] = _elements[i]!;
            }
        }

        public void Clear()
        {
            for (int i = 0; i < Count; i++)
            {
                _elements[i] = null;
            }
        }

        public IEnumerator<ICommand> GetEnumerator()
        {
            for (int i = 0; i < Count; i++)
            {
                yield return _elements[i]!;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
