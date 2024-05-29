using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    private SoundManager soundManager;

    void Start()
    {
        soundManager = FindObjectOfType<SoundManager>();
        if (soundManager == null)
        {
            Debug.LogError("SoundManager is not found!");
        }
        DontDestroyOnLoad(gameObject); // SceneChanger 오브젝트를 씬 전환 시에도 유지
    }

    public void ChangeScene(string sceneName, string newBgmPath)
    {
        StartCoroutine(FadeOutBgmAndLoadScene(sceneName, newBgmPath));
    }

    public void LobbyScene()
    {
        StartCoroutine(FadeOutBgmAndLoadScene("GameLobby", "Sound/GameLobby_Sleepy Sunshine"));
    }

    public void RoomScene() 
    {
        StartCoroutine(FadeOutBgmAndLoadScene("GameRoom", "Sound/GameLobby_Sleepy Sunshine")); // Bgm 바꿔야함.
        Debug.Log("Successfully joined room");
    }

    private IEnumerator FadeOutBgmAndLoadScene(string sceneName, string newBgmPath)
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

        // 씬 로드
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // 새로운 BGM 로드 및 재생
        AudioClip newBgmClip = Resources.Load<AudioClip>(newBgmPath);
        if (newBgmClip != null)
        {
            bgmSource.clip = newBgmClip;
            bgmSource.volume = 0;
            bgmSource.Play();

            // 페이드 인
            for (float t = 0; t < 1; t += Time.deltaTime)
            {
                bgmSource.volume = Mathf.Lerp(0, 1.0f, t / 1f);
                yield return null;
            }

            bgmSource.volume = 1.0f;
        }
        else
        {
            Debug.LogWarning($"AudioClip not found at path: {newBgmPath}");
        }
    }
}