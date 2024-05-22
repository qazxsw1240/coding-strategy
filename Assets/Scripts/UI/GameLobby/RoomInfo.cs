using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace CodingStrategy.UI.GameLobby
{
    public class RoomInfo : MonoBehaviour
    {
        public TMP_Text modeTitle;
        public TMP_Text roomName;
        public TMP_Text playerCount;

        public void setModeTitle(string modename)
        {
            modeTitle.SetText(modename);
        }

        public void setRoomName(string roomname)
        {
            roomName.SetText(roomname);
        }

        public void setPlayerCount(int count)
        {
            playerCount.SetText(count.ToString() + " /4");
        }

        // Start is called before the first frame update
        void Start() {}

        // Update is called once per frame
        void Update() {}
    }
}
