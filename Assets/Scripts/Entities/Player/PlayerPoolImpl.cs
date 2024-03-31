namespace CodingStrategy.Entities.Player
{
    using System.Collections.Generic;

    public class PlayerPoolImpl : IPlayerPool
    {
        private readonly IDictionary<string, IPlayerDelegate> _pool;
        public PlayerPoolImpl(
            IPlayerDelegate Player1,
            IPlayerDelegate Player2,
            IPlayerDelegate Player3,
            IPlayerDelegate Player4
            )
        {
            _pool = new Dictionary<string, IPlayerDelegate>
            {
                { Player1.Id, Player1 },
                { Player2.Id, Player2 },
                { Player3.Id, Player3 },
                { Player4.Id, Player4 }
            };
        }
        public IPlayerDelegate this[string id]
        {
            get => _pool[id];
        }
        public IDictionary<string, IPlayerDelegate> PlayerPool => _pool;
    }
}
