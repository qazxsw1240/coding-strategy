#nullable enable


using CodingStrategy.Utility;

namespace CodingStrategy.Entities.Runtime
{
    using System.Diagnostics.CodeAnalysis;
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class ExecutionQueueImpl : IExecutionQueue
    {
        private static readonly IComparer<IStatement> StatementComparer = new StatementComparer();

        private readonly PriorityQueue<IStatement> _statements;

        public ExecutionQueueImpl()
        {
            _statements = new PriorityQueue<IStatement>(StatementComparer);
        }

        public ExecutionQueueImpl(IEnumerable<IStatement> statements)
        {
            _statements = new PriorityQueue<IStatement>(StatementComparer);
            foreach (IStatement statement in statements)
            {
                _statements.Enqueue(statement);
            }
        }


        public int Count => _statements.Count;

        public bool IsReadOnly => false;

        public void Add(IStatement item)
        {
            Enqueue(item);
        }

        public bool Contains(IStatement item)
        {
            return false;
        }

        public void Enqueue(IStatement statement)
        {
            _statements.Enqueue(statement);
        }

        public IStatement Dequeue()
        {
            return _statements.Dequeue();
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
            return _statements.TryDequeue(out statement);
        }

        public void Clear()
        {
            _statements.Clear();
        }

        public void CopyTo(IStatement[] array, int arrayIndex)
        {
            return;
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

    public class StatementComparer : IComparer<IStatement>
    {
        public int Compare(IStatement x, IStatement y)
        {
            if (ReferenceEquals(x, y)) return 0;
            if (ReferenceEquals(null, y)) return 1;
            if (ReferenceEquals(null, x)) return -1;
            return x.Phase.CompareTo(y.Phase);
        }
    }
}
