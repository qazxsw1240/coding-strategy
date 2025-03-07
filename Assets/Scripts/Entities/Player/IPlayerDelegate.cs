using CodingStrategy.Entities.Robot;

using UnityEngine.Events;

namespace CodingStrategy.Entities.Player
{
    /// <summary>
    ///     현재 게임의 플레이어 상태를 저장하며, 플레이어 상태를 변경할 수 있습니다.
    /// </summary>
    public interface IPlayerDelegate : IGameEntity
    {
        /// <summary>
        ///     플레이어의 아이디입니다.
        /// </summary>
        public new abstract string ID { get; }

        /// <summary>
        ///     플레이어의 현재 체력입니다.
        /// </summary>
        public abstract int HealthPoint { get; set; }

        /// <summary>
        ///     플레이어의 레벨입니다.
        /// </summary>
        public abstract int Level { get; set; }

        /// <summary>
        ///     플레이어의 경험치입니다.
        /// </summary>
        public abstract int Exp { get; set; }

        /// <summary>
        ///     플레이어의 재화량입니다.
        /// </summary>
        public abstract int Currency { get; set; }

        /// 플레이어의 로봇 대리자입니다.
        public abstract IRobotDelegate Robot { get; set; }

        /// <summary>
        ///     플레이어의 알고리즘입니다.
        /// </summary>
        public abstract IAlgorithm Algorithm { get; }

        public abstract UnityEvent<int, int> OnHealthPointChange { get; }

        public abstract UnityEvent<int, int> OnLevelChange { get; }

        public abstract UnityEvent<int, int> OnExpChange { get; }

        public abstract UnityEvent<int, int> OnCurrencyChange { get; }
    }
}
