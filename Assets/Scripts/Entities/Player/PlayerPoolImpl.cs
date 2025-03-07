using System;
using System.Collections;
using System.Collections.Generic;

namespace CodingStrategy.Entities.Player
{
    public class PlayerPoolImpl : IPlayerPool
    {
        /// <summary>
        ///     최대 플레이어 수입니다.
        /// </summary>
        private const int MaxPlayers = 4;

        private readonly IDictionary<string, IPlayerDelegate> _pool = new Dictionary<string, IPlayerDelegate>();

        [Obsolete]
        public IDictionary<string, IPlayerDelegate> PlayerPool
        {
            get => _pool;
        }

        public IPlayerDelegate this[string id]
        {
            get => _pool[id];
            set => _pool[id] = value;
        }

        public void Add(string id, Func<string, IPlayerDelegate> generator)
        {
            IPlayerDelegate obj = generator(id);
            _pool[id] = obj;
        }

        public bool Contains(string id)
        {
            return _pool.ContainsKey(id);
        }

        /// <summary>
        ///     플레이어 풀에서 특정 플레이어를 제거합니다.
        /// </summary>
        /// <param name="id">제거할 플레이어의 아이디입니다.</param>
        /// <exception cref="KeyNotFoundException">아이디가 존재하지 않을 경우 예외를 발생시킵니다.</exception>
        public void Remove(string id)
        {
            if (!_pool.ContainsKey(id))
            {
                throw new KeyNotFoundException();
            }

            _pool.Remove(id);
        }

        /// <summary>
        ///     플레이어 풀을 초기화합니다.
        /// </summary>
        public void Clear()
        {
            _pool.Clear();
        }

        public IEnumerator<IPlayerDelegate> GetEnumerator()
        {
            return _pool.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        ///     플레이어 풀에 플레이어를 추가합니다.
        /// </summary>
        /// <param name="id">플레이어 아이디입니다.</param>
        /// <param name="player">플레이어 정보 대리자입니다.</param>
        /// <exception cref="Exception"></exception>
        [Obsolete]
        public void Add(string id, IPlayerDelegate player)
        {
            if (_pool.Count == MaxPlayers)
            {
                throw new Exception();
            }

            _pool.Add(id, player);
        }
    }
}
