#nullable enable


namespace CodingStrategy.Entities.Runtime.Abnormality
{
    using CodingStrategy.Entities.Player;
    using Robot;
    public interface IAbnormality
    {
        public abstract string Name { get; }
        public abstract IRobotDelegate RobotDelegate { get; }
        public abstract IPlayerDelegate? PlayerDelegate { get; set; }
        public abstract int Value { get; set;}
        /// <summary>
        /// 매턴 종료시 실행되는 메서드입니다.
        /// </summary>
        public abstract void Execute();
        public abstract IAbnormality Copy(IRobotDelegate robotDelegate, bool keepStatus = false);

    }
}