#nullable enable


namespace CodingStrategy.Runtime
{
    using System.Collections;
    using System.Collections.Generic;
    using CodingStrategy.Entities.Robot;

    public class ExecutionQueuePool : IDictionary<IRobotDelegate, IExecutionQueue>
    {
        private readonly IDictionary<IRobotDelegate, IExecutionQueue> _executionQueues;

        public ExecutionQueuePool()
        {
            _executionQueues = new Dictionary<IRobotDelegate, IExecutionQueue>();
        }


        public IExecutionQueue this[IRobotDelegate key]
        {
            get => _executionQueues[key];
            set => _executionQueues[key] = value;
        }

        public ICollection<IRobotDelegate> Keys => _executionQueues.Keys;

        public ICollection<IExecutionQueue> Values => _executionQueues.Values;

        public int Count => _executionQueues.Count;

        public bool IsReadOnly => _executionQueues.IsReadOnly;

        public void Add(IRobotDelegate key, IExecutionQueue value) => _executionQueues.Add(key, value);

        public void Add(KeyValuePair<IRobotDelegate, IExecutionQueue> pair) => _executionQueues.Add(pair);

        public void Clear() => _executionQueues.Clear();

        public bool Contains(KeyValuePair<IRobotDelegate, IExecutionQueue> pair) => Contains(pair);

        public bool ContainsKey(IRobotDelegate robotDelegate) => ContainsKey(robotDelegate);

        public void CopyTo(KeyValuePair<IRobotDelegate, IExecutionQueue>[] array, int arrayIndex) => CopyTo(array, arrayIndex);

        public IEnumerator<KeyValuePair<IRobotDelegate, IExecutionQueue>> GetEnumerator() => _executionQueues.GetEnumerator();

        public bool Remove(IRobotDelegate robotDelegate) => _executionQueues.Remove(robotDelegate);

        public bool Remove(KeyValuePair<IRobotDelegate, IExecutionQueue> pair) => _executionQueues.Remove(pair);

        public bool TryGetValue(IRobotDelegate key, out IExecutionQueue value) => _executionQueues.TryGetValue(key, out value);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
