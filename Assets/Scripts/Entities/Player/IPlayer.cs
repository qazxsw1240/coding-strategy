namespace CodingStrategy.Entities.Player
{
    using CodingStrategy.Entities.Robot;
    /// <summary>
    /// 현재 게임의 플레이어 상태를 저장합니다.
    /// </summary>
    public interface IPlayer
    {
        /// <summary>
        /// 플레이어의 아이디입니다.
        /// </summary>
        public abstract string Id { get; }
        /// <summary>
        /// 플레이어의 현재 체력입니다.
        /// </summary>
        public abstract int HealthPoint { get; }
        /// <summary>
        /// 플레이어의 레벨입니다.
        /// </summary>
        public abstract int Level { get; }    
        /// <summary>
        /// 플레이어의 경험치입니다.
        /// </summary>
        public abstract int Exp { get; }
        /// <summary>
        /// 플레이어의 재화량입니다.
        /// </summary>
        public abstract int Currency { get; }
        /// <summary>
        /// 플레이어의 로봇입니다.
        /// </summary>
        public abstract IRobotDelegate Robot { get; }
        /// <summary>
        /// 플레이어의 알고리즘입니다.
        /// </summary>
        public abstract IAlgorithm Algorithm { get; }
    }
}
