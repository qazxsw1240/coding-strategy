using CodingStrategy.Sound;

using UnityEngine;
using UnityEngine.UI;

namespace CodingStrategy.UI.GameLobby
{
    public class GameLobbySound : MonoBehaviour
    {
        public GameObject RoomEnterBtn;
        public GameObject RoomRandomBtn;
        private SceneChanger sceneChanger;
        private SoundManager soundManager;

        private void Awake()
        {
            soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
            sceneChanger = GameObject.Find("SoundManager").GetComponent<SceneChanger>();
        }

        // Start is called before the first frame update
        private void Start()
        {
            StartCoroutine(sceneChanger.SoundsVolumesUp("Sound/GameLobby_Sleepy Sunshine"));

            RoomEnterBtn = GameObject.Find("EnterRoom");

            Button enterButton = RoomEnterBtn.GetComponent<Button>();

            enterButton.onClick.AddListener(soundManager.OnStartButtonClickSounds);

            Button roomRandomButton = GameObject.Find("RandomEnterBtn").GetComponent<Button>();

            roomRandomButton.onClick.AddListener(soundManager.LobbyRoomButtonSound);
        }
    }
}
