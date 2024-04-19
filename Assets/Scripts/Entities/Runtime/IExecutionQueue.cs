#nullable enable



namespace CodingStrategy.Runtime
{
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// IStatement를 관리하는 큐입니다.
    /// </summary>
    public interface IExecutionQueue : ICollection<IStatement>
    {
        /// <summary>
        /// 큐에 새 IStatement 인스턴스를 추가합니다.
        /// </summary>
        /// <param name="statement">큐에 추가할 IStatement 인스턴스입니다.</param>
        public abstract void Enqueue(IStatement statement);

        /// <summary>
        /// 큐에 가장 먼저 추가된 IStatement 인스턴스를 삭제하고 반환합니다.
        /// </summary>
        /// <returns>큐에 가장 먼저 추가된 IStatement 인스턴스입니다.</returns>
        public abstract IStatement Dequeue();

        /// <summary>
        /// 큐에 가장 먼저 추가된 IStatement 인스턴스 삭제를 시도합니다.
        /// </summary>
        /// <param name="statement">성공적으로 큐에서 삭제됐을 때 저장될 매개변수입니다.</param>
        /// <returns>삭제에 성공하면 true, 큐가 비어 있어 삭제에 실패하면 false를 반환합니다</returns>
        public abstract bool TryDequeue(out IStatement statement);
    }
}
