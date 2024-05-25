using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatSoundManager : MonoBehaviour
{
    private SoundManager soundManager;

    public void ChatSound()
    {
        soundManager = FindObjectOfType<SoundManager>();
        soundManager.Init();
        // 효과음을 불러오고 재생합니다.
        AudioClip effectClip = Resources.Load<AudioClip>("Sound/Keyboard_Click_Sound");
        soundManager.Play(effectClip, Sound.Effect, 1.0f);
        Debug.Log("Chatting sound is comming out!");
    }

    public void SendMsgBtnClickSound()
    {
        soundManager = FindObjectOfType<SoundManager>();
        soundManager.Init();
        // 효과음을 불러오고 재생합니다.
        AudioClip effectClip = Resources.Load<AudioClip>("Sound/GameLobby_UI_ClickSound");
        soundManager.Play(effectClip, Sound.Effect, 1.0f);
        Debug.Log("Send button sound is comming out!");
    }

}
