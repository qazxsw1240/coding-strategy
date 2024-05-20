#nullable enable


using CodingStrategy.Utility;
using NUnit.Framework;

namespace CodingStrategy.Entities.Runtime
{
    using System.Diagnostics.CodeAnalysis;
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class ExecutionQueueImpl : IExecutionQueue
    {
        private static readonly IComparer<IStatement> StatementComparer = new StatementComparer();

        private readonly IList<IStatement> _statements;

        public ExecutionQueueImpl()
        {
            _statements = new List<IStatement>();
        }

        public ExecutionQueueImpl(IEnumerable<IStatement> statements)
        {
            _statements = new List<IStatement>();
            foreach (IStatement statement in statements)
            {
                Enqueue(statement);
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
            return _statements.Contains(item);
        }

        public void Enqueue(IStatement statement)
        {
            _statements.Add(statement);
        }

        public IStatement Dequeue()
        {
            if (!TryDequeue(out IStatement? statement))
            {
                throw new Exception();
            }

            return statement;
        }

        public void EnqueueFirst(IStatement statement)
        {
            _statements.Insert(0, statement);
        }

        public bool Remove(IStatement item)
        {
            return _statements.Remove(item);
        }

        public bool TryDequeue([MaybeNullWhen(false)] out IStatement statement)
        {
            if (_statements.Count == 0)
            {
                statement = null!;
                return false;
            }

            int index = 0;

            for (int i = 1; i < _statements.Count; i++)
            {
                IStatement minStatement = _statements[index];
                IStatement currentStatement = _statements[i];
                if (currentStatement.Phase < minStatement.Phase)
                {
                    index = i;
                }
            }

            IStatement remove = _statements[index];

            _statements.RemoveAt(index);

            statement = remove;

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
