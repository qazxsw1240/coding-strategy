#nullable enable


namespace CodingStrategy.Entities
{
    /// <summary>
    /// 명령어 정보를 나타내는 클래스입니다.
    /// </summary>
    public class CommandInfoImpl : ICommandInfo
    {
        private readonly string _name;
        private int _enhancedLevel;
        private int _grade;
        private string _explanation;

        /// <summary>
        /// 명령어 정보를 생성합니다.
        /// </summary>
        /// <param name="name">명령어의 이름입니다.</param>
        /// <param name="enhancedLevel">명령어의 강화 단계입니다.</param>
        public CommandInfoImpl(string name, int enhancedLevel, int grade, string explanation)
        {
            _name = name;
            _enhancedLevel = enhancedLevel;
            _grade = grade;
            _explanation = explanation;
        }

        public virtual string Name => _name;

        public virtual int EnhancedLevel
        {
            get => _enhancedLevel;
            set => _enhancedLevel = value;
        }

        public virtual int Grade => _grade;

        public string Explanation => _explanation;
    }
}
