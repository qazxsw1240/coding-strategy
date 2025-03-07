using CodingStrategy.Sound;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace CodingStrategy.UI
{
    public class RoomSounds : MonoBehaviour
    {
        [SerializeField] private SoundManager soundManager;

        [SerializeField] private SceneChanger sceneChanger;

        public TMP_InputField Chattings;
        public Button ReadyBtn;
        public Button StartBtn;

        private void Awake()
        {
            soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
            sceneChanger = GameObject.Find("SoundManager").GetComponent<SceneChanger>();

            Chattings = GameObject.Find("ChatInputField").GetComponent<TMP_InputField>();

            ReadyBtn = GameObject.Find("GameReadyBtn").GetComponent<Button>();
            StartBtn = GameObject.Find("GameStartBtn").GetComponent<Button>();
        }

        private void Start()
        {
            Chattings.onValueChanged.AddListener(soundManager.OnNicknameChangedSounds);

            ReadyBtn.onClick.AddListener(soundManager.LobbyRoomButtonSound);
            StartBtn.onClick.AddListener(soundManager.LobbyRoomButtonSound);
        }
    }
}
