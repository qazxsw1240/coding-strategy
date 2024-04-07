namespace CodingStrategy.Entities.Shop
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    public class CommandListImpl : ICommandList
    {
        /// <summary>
        /// 명령어를 저장하는 실제 리스트입니다.
        /// </summary>
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
            set => _commandList[index] = value;
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
        /// <summary>
        /// 확률에 따라 랜덤으로 명령어를 추출하여 반환합니다.
        /// </summary>
        /// <returns>추출한 명령어를 반환합니다.</returns>
        public ICommand SelectRandomCommand()
        {
            // 리스트가 비어있을 경우 null을 반환합니다.
            if (_commandList.Count == 0)
            {
                return null;
            }

            // 리스트에서 명령어를 랜덤으로 추출합니다.
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