using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLobbyEnterBtnSound : MonoBehaviour
{
    private SoundManager soundManager;
    //private SceneChanger sceneChanger;
    public void GameLobbyEnterBtnClicked()
    {
        soundManager = FindObjectOfType<SoundManager>();
        soundManager.Init();
        //sceneChanger = FindObjectOfType<SceneChanger>();
        //if (sceneChanger == null)
        //{
        //    GameObject sceneChangerObj = new GameObject("SceneChanger");
        //    sceneChanger = sceneChangerObj.AddComponent<SceneChanger>();
        //}

        // 효과음을 불러오고 재생합니다.
        AudioClip effectClip = Resources.Load<AudioClip>("Sound/GameRoom_GameStartSound");
        soundManager.Play(effectClip, Sound.Effect, 1.0f);
        Debug.Log("Start button sound is comming out!");
    }
}
