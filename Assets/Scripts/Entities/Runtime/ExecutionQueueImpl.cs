#nullable enable


namespace CodingStrategy.Runtime
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class ExecutionQueueImpl : IExecutionQueue
    {
        private const int DefaultCapacity = 10;

        private readonly Queue<IStatement> _statements;

        public ExecutionQueueImpl(int capacity) =>
            _statements = new Queue<IStatement>(capacity);

        public ExecutionQueueImpl(IEnumerable<IStatement> statements) =>
            _statements = new Queue<IStatement>(statements);

        public ExecutionQueueImpl() : this(DefaultCapacity)
        {
        }


        public int Count => _statements.Count;

        public bool IsReadOnly => false;

        public void Add(IStatement item) => Enqueue(item);

        public bool Contains(IStatement item) => _statements.Contains(item);

        public void Enqueue(IStatement statement) => _statements.Enqueue(statement);

        public IStatement Dequeue() => _statements.Dequeue();

        public bool Remove(IStatement item) => throw new NotImplementedException();

        public bool TryDequeue(out IStatement statement) => _statements.TryDequeue(out statement);

        public void Clear() => _statements.Clear();

        public void CopyTo(IStatement[] array, int arrayIndex) => _statements.CopyTo(array, arrayIndex);

        public IEnumerator<IStatement> GetEnumerator() => _statements.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    }
}
