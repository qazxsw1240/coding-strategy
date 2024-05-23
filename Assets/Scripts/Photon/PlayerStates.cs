using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace CodingStrategy.PlayerStates
{
    // Room Scene에 정보를 전달할 오브젝트가 될 것입니다.
    // OnJoinedRoom() 함수 뒤에 바로 OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)가 실행되어버려서
    // 놀랍게도 둘이 따로 두면 아예 작동이 되지 않는 것을 3시간에 걸쳐 깨달아버렸습니다.
    // 고로 우회하기 위해 해당 오브젝트에 정보를 저장해서 GameRoom쪽으로 보내버릴겁니다.

    public class PlayerStates : MonoBehaviourPun
    {
        [SerializeField] public List<Player> playersinRoom = null;
        [SerializeField] public bool[] ready = new bool[4];

        public void Start()
        {
            if (playersinRoom == null)
            {
                playersinRoom=new List<Player>();
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                DontDestroyOnLoad(gameObject);
            }
        }

        [PunRPC]
        public void UpdatePlayerStates(Player player, bool torF)
        {
            if (torF) 
            {
                playersinRoom.Add(player);
                Debug.Log(playersinRoom[0].NickName);
            }
            else { playersinRoom.Remove(player); }
        }
    }


}
    