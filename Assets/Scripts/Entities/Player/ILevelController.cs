#nullable enable

namespace CodingStrategy.Entities.Player
{
    using System;
    public class ILevelController
    {
        private readonly PlayerPoolImpl _playerPoolImpl;
        public ILevelController(PlayerPoolImpl playerPoolImpl)
        {
            _playerPoolImpl=playerPoolImpl;
        }
        public void IncreaseExp(string id, int value)
        {
            IPlayerDelegate player=_playerPoolImpl[id];

            throw new NotImplementedException();
        }
    }
}
