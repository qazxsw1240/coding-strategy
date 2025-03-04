using System.Collections;

using UnityEngine;

namespace CodingStrategy.Sound
{
    public class SceneChanger : MonoBehaviour
    {
        [SerializeField] private SoundManager soundManager;

        public void Start()
        {
            soundManager = FindObjectOfType<SoundManager>();
            if (soundManager == null)
            {
                Debug.LogError("SoundManager is not found!");
            }

            DontDestroyOnLoad(gameObject); // SceneChanger 오브젝트를 씬 전환 시에도 유지
        }

        public void LobbyScene()
        {
            StartCoroutine(SoundsVolumesDown());
        }

        public IEnumerator SoundsVolumesDown()
        {
            AudioSource bgmSource = soundManager.GetBgmSource();
            float startVolume = bgmSource.volume;

            // 페이드 아웃
            for (float t = 0; t < 1; t += Time.deltaTime)
            {
                bgmSource.volume = Mathf.Lerp(startVolume, 0, t / 1f);
                yield return null;
            }

            bgmSource.volume = 0;
            bgmSource.Stop();
        }

        public void GameScene()
        {
            StartCoroutine(SoundsVolumesDown());
        }

        public IEnumerator SoundsVolumesUp(string newBgmPath)
        {
            AudioSource bgmSource = soundManager.GetBgmSource();
            AudioClip newBgmClip = Resources.Load<AudioClip>(newBgmPath);
            if (newBgmClip != null)
            {
                bgmSource.clip = newBgmClip;
                bgmSource.volume = 0;
                bgmSource.Play();

                // 페이드 인
                for (float t = 0; t < 1; t += Time.deltaTime)
                {
                    bgmSource.volume = Mathf.Lerp(0, 0.2f, t / 1f);
                    yield return null;
                }

                bgmSource.volume = 0.2f;
            }
            else
            {
                Debug.LogWarning($"AudioClip not found at path: {newBgmPath}");
            }
        }
    }
}
