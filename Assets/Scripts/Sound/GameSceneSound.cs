using UnityEngine;

namespace CodingStrategy.Sound
{
    public class GameSceneSound : MonoBehaviour
    {
        [SerializeField] private SceneChanger sceneChanger;

        private void Awake()
        {
            sceneChanger = GameObject.Find("SoundManager").GetComponent<SceneChanger>();
        }

        private void Start()
        {
            StartCoroutine(sceneChanger.SoundsVolumesUp("Sound/GameScene_Bgm"));
        }
    }
}
