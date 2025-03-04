using CodingStrategy.Sound;

using UnityEngine;

namespace CodingStrategy.Prefabs.UI.GameLobby
{
    public class RoomBtnConnectSoundsOnCreate : MonoBehaviour
    {
        public GameObject createdBtn;
        private SceneChanger _sceneChanger;
        private ISoundManager _soundManager;

        private void Awake()
        {
            _soundManager = SoundManager.Instance;
            _sceneChanger = GameObject.Find("SoundManager").GetComponent<SceneChanger>();
        }

        // Start is called before the first frame update
        private void Start()
        {
            createdBtn = gameObject;
        }
    }
}
