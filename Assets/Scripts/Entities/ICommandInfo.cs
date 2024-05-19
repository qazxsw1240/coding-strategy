#nullable enable


namespace CodingStrategy.Entities
{
    /// <summary>
    /// 명령어의 정보를 나타내는 인터페이스입니다.
    /// </summary>
    public interface ICommandInfo
    {
        /// <summary>
        /// 명령어의 이름입니다.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// 명령어가 강화된 단계입니다.
        /// </summary>
        public abstract int EnhancedLevel { get; set; }

        /// <summary>
        /// 명령어의 등급입니다.
        /// </summary>
        public abstract int Grade { get; }
    }
}
