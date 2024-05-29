using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class RoomBtnConnectSoundsOnCreate : MonoBehaviour
{
    SoundManager soundmanager;
    SceneChanger sceneChanger;

    public GameObject CreatedBtn;

    private void Awake()
    {
        soundmanager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        sceneChanger = GameObject.Find("SoundManager").GetComponent<SceneChanger>();
        //soundmanager.Init();
    }

    // Start is called before the first frame update
    void Start()
    {
        CreatedBtn = gameObject;

        Button EnterBtn = CreatedBtn.GetComponent<Button>();

        EnterBtn.onClick.AddListener(soundmanager.LobbyRoomButtonSound);
    }
}

