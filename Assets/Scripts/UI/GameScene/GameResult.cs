using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

namespace CodingStrategy.UI.InGame
{
    public class GameResult : MonoBehaviourPunCallbacks
    {
        public GameObject Gameresult;
        public GameObject EndCard;
        public TextMeshProUGUI Rank;
        public TextMeshProUGUI EndingMessage;
    
        public Button QuitRoomBtn;
    
        public void Awake()
        {
            QuitRoomBtn = GameObject.Find("Quit Room Button").GetComponent<Button>();
            QuitRoomBtn.onClick.AddListener(PlayerLeave);
        }
    
        public void PlayerLeave()
        {
            SceneManager.LoadScene("GameLobby");
        }
    }
}
