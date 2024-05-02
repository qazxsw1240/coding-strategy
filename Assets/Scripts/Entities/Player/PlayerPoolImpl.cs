namespace CodingStrategy.Entities.Player
{
    using System;
    using System.Collections.Generic;

    public class PlayerPoolImpl : IPlayerPool
    {
        /// <summary>
        /// 최대 플레이어 수입니다.
        /// </summary>
        private const int MaxPlayers = 4;
        private readonly IDictionary<string, IPlayerDelegate> _pool;
        public PlayerPoolImpl()
        {
            _pool = new Dictionary<string, IPlayerDelegate>();
        }
        public IPlayerDelegate this[string id]
        {
            get => _pool[id];
            set => _pool[id] = value;
        }
        public IDictionary<string, IPlayerDelegate> PlayerPool => _pool;
        /// <summary>
        /// 플레이어 풀에 플레이어를 추가합니다.
        /// </summary>
        /// <param name="id">플레이어 아이디입니다.</param>
        /// <param name="player">플레이어 정보 대리자입니다.</param>
        /// <exception cref="Exception"></exception>
        public void Add(string id, IPlayerDelegate player)
        {
            if(_pool.Count == MaxPlayers)
            {
                throw new Exception();
            }
            _pool.Add(id, player);
        }
        /// <summary>
        /// 플레이어 풀에서 특정 플레이어를 제거합니다.
        /// </summary>
        /// <param name="id">제거할 플레이어의 아이디입니다.</param>
        /// <exception cref="Exception">아이디가 존재하지 않을 경우 예외를 발생시킵니다.</exception>
        public void Remove(string id)
        {
            if(!_pool.ContainsKey(id))
            {
                throw new Exception();
            }
            _pool.Remove(id);
        }
        /// <summary>
        /// 플레이어 풀을 초기화합니다.
        /// </summary>
        public void Clear()
        {
            _pool.Clear();
        }
    }
}
