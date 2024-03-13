#nullable enable

namespace CodingStrategy.Entities.Player
{
    using System;
    public class LevelController
    {
        private readonly IPlayerPool _playerPool;
        public LevelController(PlayerPoolImpl playerPoolImpl)
        {
            _playerPool=playerPoolImpl;
        }
        public void IncreaseExp(string id, int value)
        {
            IPlayerDelegate player=_playerPool[id];

            throw new NotImplementedException();
        }
    }
}
