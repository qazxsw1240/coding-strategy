using TMPro;

using UnityEngine;

namespace CodingStrategy.UI.GameLobby
{
    public class RoomInfo : MonoBehaviour
    {
        public TMP_Text modeTitle;
        public TMP_Text roomName;
        public TMP_Text playerCount;

        public void SetModeTitle(string modeName)
        {
            modeTitle.SetText(modeName);
        }

        public void SetRoomName(string roomName)
        {
            this.roomName.SetText(roomName);
        }

        public void SetPlayerCount(int count)
        {
            playerCount.SetText($"{count}/4");
        }
    }
}
