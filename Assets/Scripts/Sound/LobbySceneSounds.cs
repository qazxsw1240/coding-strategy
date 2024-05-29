using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbySceneSounds : MonoBehaviour
{
    SoundManager soundmanager;
    SceneChanger sceneChanger;

    public GameObject RoomEnterBtn;

    private void Awake()
    {
        soundmanager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        sceneChanger = GameObject.Find("SoundManager").GetComponent<SceneChanger>();
        soundmanager.Init();
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(sceneChanger.SoundsVolumesUp("Sound/GameLobby_Sleepy Sunshine"));

        RoomEnterBtn = GameObject.Find("EnterRoom");

        Button EnterBtn = RoomEnterBtn.GetComponent<Button>();

        EnterBtn.onClick.AddListener(soundmanager.OnStartButtonClick);
    }
}
