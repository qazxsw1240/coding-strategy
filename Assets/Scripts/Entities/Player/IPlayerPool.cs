namespace CodingStrategy.Entities.Player
{
    using System.Collections.Generic;
    /// <summary>
    /// 플레이어 풀입니다. 플레이어의 아이디로 해당 플레이어의 상태에 접근할 수 있습니다.
    /// </summary>
    public interface IPlayerPool
    {
        /// <summary>
        /// 플레이어 아이디를 키로, 플레이어의 상태를 값으로 저장하는 딕셔너리입니다.
        /// </summary>
        public abstract IDictionary<string, IPlayerDelegate> PlayerPool { get; }
        /// <summary>
        /// 플레이어 아이디로 해당 플레이어의 상태에 접근합니다.
        /// </summary>
        public abstract IPlayerDelegate this[string id] { get; set; }
        /// <summary>
        /// 플레이어 풀에서 특정 플레이어를 제거합니다.
        /// </summary>
        /// <param name="id">제거할 플레이어의 아이디입니다.</param>
        public abstract void Remove(string id);
    }
}
