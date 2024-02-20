#nullable enable


namespace CodingStrategy.Runtime
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
    }
}
