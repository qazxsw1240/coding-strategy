using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CodingStrategy.Entities
{
    /// <summary>
    /// 인게임에 필요한 명령어를 생성하는 명령어 풀입니다. 생성한 명령어는 0부터 시작하는 생성번호가 ID로 제공됩니다.
    /// </summary>
    public class InGameCommandPool : ICommandPool
    {
        private readonly IDictionary<string, ICommand> _commands;
        private readonly IDictionary<string, int> _commandCounter;

        /// <summary>
        /// 주어진 명령어로 명령어 풀을 생성합니다.
        /// </summary>
        /// <param name="commands">명령어 풀이 지닐 명령어 컬렉션입니다.</param>
        public InGameCommandPool(Collection<ICommand> commands)
        {
            _commands = new Dictionary<string, ICommand>();
            _commandCounter = new Dictionary<string, int>();

            foreach (ICommand command in commands)
            {
                _commands[command.Id] = command;
            }
        }

        public int Count => _commands.Count;

        public ICommand this[string key] => _commands[key];

        public IEnumerable<string> Keys => _commands.Keys;

        public IEnumerable<ICommand> Values => _commands.Values;

        public bool ContainsKey(string key) => _commands.ContainsKey(key);

        public bool TryGetValue(string key, out ICommand value) => _commands.TryGetValue(key, out value);

        public ICommand Create(string id)
        {
            ICommand command = this[id].Copy(false);
            if (!_commandCounter.ContainsKey(id))
            {
                _commandCounter[id] = 0;
            }
            int count = _commandCounter[id]++;
            command.Id = $"{id}-{count}";
            return command;
        }

        public IEnumerator<KeyValuePair<string, ICommand>> GetEnumerator() => _commands.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
