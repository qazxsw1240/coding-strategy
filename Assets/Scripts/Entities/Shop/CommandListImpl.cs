namespace CodingStrategy.Entities.Shop
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    public class CommandListImpl : ICommandList
    {
        private readonly List<ICommand> _commandList;
        private Random _random;

        public CommandListImpl()
        {
            _commandList = new List<ICommand>();
            _random = new Random();
        }
        public ICommand this[int index]
        {
            get => _commandList[index];
            set => throw new System.NotImplementedException();
        }
        public int Count => _commandList.Count;

        public bool IsReadOnly => true;

        public void Add(ICommand item)
        {
            _commandList.Add(item);
        }

        public void Clear()
        {
            _commandList.Clear();
        }

        public bool Contains(ICommand item)
        {
            return _commandList.Contains(item);
        }

        public void CopyTo(ICommand[] array, int arrayIndex)
        {
            _commandList.CopyTo(array,arrayIndex);
        }

        public IEnumerator<ICommand> GetEnumerator()
        {
            return _commandList.GetEnumerator();
        }

        public int IndexOf(ICommand item)
        {
            return _commandList.IndexOf(item);
        }

        public void Insert(int index, ICommand item)
        {
            _commandList.Insert(index,item);
        }

        public bool Remove(ICommand item)
        {
            return _commandList.Remove(item);
        }

        public void RemoveAt(int index)
        {
            _commandList.RemoveAt(index);
        }

        public ICommand SelectRandomCommand()
        {
            if (_commandList.Count == 0)
            {
                return null;
            }
            int randomIndex = _random.Next(_commandList.Count);
            ICommand selectedCommand = _commandList[randomIndex];
            _commandList.RemoveAt(randomIndex);
            return selectedCommand;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _commandList.GetEnumerator();
        }
    }
}