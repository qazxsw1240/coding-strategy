#nullable enable


namespace CodingStrategy.Entities.Runtime
{
    using System.Diagnostics.CodeAnalysis;
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class ExecutionQueueImpl : IExecutionQueue
    {
        private const int DefaultCapacity = 10;

        private readonly List<IStatement> _statements;

        public ExecutionQueueImpl(int capacity)
        {
            _statements = new List<IStatement>(capacity);
        }

        public ExecutionQueueImpl(IEnumerable<IStatement> statements)
        {
            _statements = new List<IStatement>(statements);
        }

        public ExecutionQueueImpl() : this(DefaultCapacity) {}


        public int Count => _statements.Count;

        public bool IsReadOnly => false;

        public void Add(IStatement item)
        {
            Enqueue(item);
        }

        public bool Contains(IStatement item)
        {
            return _statements.Contains(item);
        }

        public void Enqueue(IStatement statement)
        {
            _statements.Add(statement);
        }

        public IStatement Dequeue()
        {
            IStatement statement = _statements[0];
            _statements.RemoveAt(0);
            return statement;
        }

        public void EnqueueFirst(IStatement statement)
        {
            throw new NotImplementedException();
        }

        public bool Remove(IStatement item)
        {
            throw new NotImplementedException();
        }

        public bool TryDequeue([MaybeNullWhen(false)] out IStatement statement)
        {
            if (_statements.Count == 0)
            {
                statement = null;
                return false;
            }

            IStatement obsolete = _statements[0];
            _statements.RemoveAt(0);
            statement = obsolete;
            return true;
        }

        public void Clear()
        {
            _statements.Clear();
        }

        public void CopyTo(IStatement[] array, int arrayIndex)
        {
            _statements.CopyTo(array, arrayIndex);
        }

        public IEnumerator<IStatement> GetEnumerator()
        {
            return _statements.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
