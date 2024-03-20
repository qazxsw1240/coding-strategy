#nullable enable



namespace CodingStrategy.Runtime
{
    using System.Collections;
    using System.Collections.Generic;

    public interface IExecutionQueue : IReadOnlyCollection<IStatement>
    {
        public abstract void Enqueue(IStatement statement);

        public abstract IStatement Dequeue();

        public abstract bool TryDequeue(out IStatement statement);
    }
}
