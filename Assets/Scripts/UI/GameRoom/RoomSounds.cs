using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


public class RoomSounds : MonoBehaviour
{
    SoundManager soundmanager;
    SceneChanger sceneChanger;

    public TMP_InputField Chattings;

    public Button ReadyBtn;
    public Button StartBtn;
    
    private void Awake()
    {
        soundmanager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        sceneChanger = GameObject.Find("SoundManager").GetComponent<SceneChanger>();

        Chattings = GameObject.Find("ChatInputField").GetComponent<TMP_InputField>();

        ReadyBtn = GameObject.Find("GameReadyBtn").GetComponent<Button>();
        StartBtn = GameObject.Find("GameStartBtn").GetComponent<Button>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Chattings.onValueChanged.AddListener(soundmanager.OnNicknameChangedSounds);

        ReadyBtn.onClick.AddListener(soundmanager.LobbyRoomButtonSound);
        StartBtn.onClick.AddListener(soundmanager.LobbyRoomButtonSound);
    }
}
