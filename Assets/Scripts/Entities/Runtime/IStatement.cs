#nullable enable


namespace CodingStrategy.Entities.Runtime
{
    /// <summary>
    /// ExecutorQueue가 실행할 명령문 인스턴스입니다.
    /// </summary>
    public interface IStatement
    {
        /// <summary>
        /// 명령문을 실행합니다.
        /// </summary>
        public abstract void Execute();

        public abstract StatementPhase Phase { get; }

        /// <summary>
        /// 명령문과 반대로 작동하는 명령문입니다.
        /// </summary>
        public abstract IStatement Reverse { get; }
    }
}
