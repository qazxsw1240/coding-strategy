using System;
using System.Collections;
using System.Collections.Generic;

namespace CodingStrategy.Entities.Runtime
{
    public class ExecutionQueueImpl : IExecutionQueue
    {
        private readonly IList<IStatement> _statements;

        public ExecutionQueueImpl()
        {
            _statements = new List<IStatement>();
            IsProtected = false;
        }

        public ExecutionQueueImpl(IEnumerable<IStatement> statements) : this()
        {
            foreach (IStatement statement in statements)
            {
                Enqueue(statement);
            }
        }

        public int Count
        {
            get => _statements.Count;
        }

        public bool IsReadOnly
        {
            get => false;
        }

        public void Add(IStatement item)
        {
            Enqueue(item);
        }

        public bool Contains(IStatement item)
        {
            return _statements.Contains(item);
        }

        public bool IsProtected { get; set; }

        public void Enqueue(IStatement statement)
        {
            if (IsProtected)
            {
                return;
            }
            _statements.Add(statement);
        }

        public IStatement Dequeue()
        {
            if (!TryDequeue(out IStatement statement))
            {
                throw new Exception();
            }

            return statement!;
        }

        public void EnqueueFirst(IStatement statement)
        {
            if (IsProtected)
            {
                return;
            }
            _statements.Insert(0, statement);
        }

        public bool Remove(IStatement item)
        {
            return _statements.Remove(item);
        }

        public bool TryDequeue(out IStatement statement)
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
}
