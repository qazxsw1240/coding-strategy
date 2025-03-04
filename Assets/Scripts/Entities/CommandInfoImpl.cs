#nullable enable

namespace CodingStrategy.Entities
{
    /// <summary>
    ///     명령어 정보를 나타내는 클래스입니다.
    /// </summary>
    public sealed class CommandInfoImpl : ICommandInfo
    {
        /// <summary>
        ///     명령어 정보를 생성합니다.
        /// </summary>
        /// <param name="name">명령어의 이름입니다.</param>
        /// <param name="enhancedLevel">명령어의 강화 단계입니다.</param>
        /// <param name="grade"></param>
        /// <param name="explanation"></param>
        public CommandInfoImpl(string name, int enhancedLevel, int grade, string explanation)
        {
            Name = name;
            EnhancedLevel = enhancedLevel;
            Grade = grade;
            Explanation = explanation;
        }

        public string Name { get; }

        public int EnhancedLevel { get; set; }

        public int Grade { get; }

        public string Explanation { get; }
    }
}
