using CodingStrategy.Entities.Player;
using CodingStrategy.Factory;
using Photon.Pun;
using Photon.Realtime;

namespace CodingStrategy
{
    public class GameManagerUtil : MonoBehaviourPunCallbacks
    {
        public readonly IPlayerPool PlayerDelegatePool = new PlayerPoolImpl();

        public void Awake()
        {
            DontDestroyOnLoad(this);
        }

        public void InitializePlayersByPhoton()
        {
            InitializePlayersByPhoton(PhotonNetwork.CurrentRoom);
        }

        public void InitializePlayersByPhoton(Room room)
        {
            foreach ((int _, Player photonPlayer) in room.Players)
            {
                string id = photonPlayer.UserId;
                IPlayerDelegateCreateStrategy strategy = new PlayerDelegateCreateStrategy(id);
                IPlayerDelegateCreateFactory factory = new PlayerDelegateCreateFactory(strategy);
                IPlayerDelegate playerDelegate = factory.Build();
                PlayerDelegatePool[playerDelegate.Id] = playerDelegate;
            }
        }

        public void ClearPlayers()
        {
            foreach (IPlayerDelegate playerDelegate in PlayerDelegatePool)
            {
                playerDelegate.OnHealthPointChange.RemoveAllListeners();
                playerDelegate.OnCurrencyChange.RemoveAllListeners();
                playerDelegate.OnExpChange.RemoveAllListeners();
                playerDelegate.OnLevelChange.RemoveAllListeners();
            }

            PlayerDelegatePool.Clear();
        }

        public IPlayerDelegate LocalPhotonPlayerDelegate => PlayerDelegatePool[PhotonNetwork.LocalPlayer.UserId];

        public IPlayerDelegate GetPlayerDelegate(Player photonPlayer)
        {
            return PlayerDelegatePool[photonPlayer.UserId];
        }

        public void RemovePlayerDelegate(Player photonPlayer)
        {
            string id = photonPlayer.UserId;
            IPlayerDelegate playerDelegate = PlayerDelegatePool[id];
            playerDelegate.OnHealthPointChange.RemoveAllListeners();
            playerDelegate.OnCurrencyChange.RemoveAllListeners();
            playerDelegate.OnExpChange.RemoveAllListeners();
            playerDelegate.OnLevelChange.RemoveAllListeners();
            PlayerDelegatePool.Remove(id);
        }
    }
}
