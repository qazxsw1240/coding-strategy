using Photon.Realtime;

namespace CodingStrategy.Photon
{
    public static class PhotonLobbyStrategy
    {
        public static TypedLobby TypedLobby => new TypedLobby("coding-strategy", LobbyType.SqlLobby);
    }
}
