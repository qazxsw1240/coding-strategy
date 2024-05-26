using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    private SoundManager soundManager;

    private void Start()
    {
        soundManager = FindObjectOfType<SoundManager>();
        soundManager.Init();
    }

    public void ChangeScene(string sceneName, string nextSceneBgmPath)
    {
        // BGM을 변경합니다.
        soundManager.ChangeBGM(nextSceneBgmPath);

        // 씬 전환을 실행합니다.
        SceneManager.LoadScene(sceneName);
    }
}
