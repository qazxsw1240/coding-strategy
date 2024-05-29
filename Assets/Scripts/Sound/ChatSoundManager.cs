using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatSoundManager : MonoBehaviour
{
    private SoundManager soundManager;
    public InputField ChatInputField;

    //public void ChatSound()
    //{
    //    soundManager = FindObjectOfType<SoundManager>();
    //    soundManager.Init();
    //    // 효과음을 불러오고 재생합니다.
    //    AudioClip effectClip = Resources.Load<AudioClip>("Sound/Keyboard_Click_Sound");
    //    soundManager.Play(effectClip, Sound.Effect, 1.0f);
    //    Debug.Log("Chatting sound is comming out!");
    //}

    //public void SendMsgBtnClickSound()
    //{
    //    soundManager = FindObjectOfType<SoundManager>();
    //    soundManager.Init();
    //    // 효과음을 불러오고 재생합니다.
    //    AudioClip effectClip = Resources.Load<AudioClip>("Sound/GameLobby_UI_ClickSound");
    //    soundManager.Play(effectClip, Sound.Effect, 1.0f);
    //    Debug.Log("Send button sound is comming out!");
    //}

    //public void ChatSound()
    //{
    //    // Manager를 통해 SoundManager에 접근
    //    SoundManager soundManager = Manager.Sound;

    //    // SoundManager가 초기화되지 않았을 경우를 대비한 예외 처리
    //    if (soundManager == null)
    //    {
    //        Debug.LogError("SoundManager가 초기화되지 않았습니다.");
    //        return;
    //    }

    //    // 효과음을 불러오고 재생합니다.
    //    AudioClip effectClip = Resources.Load<AudioClip>("Sound/Keyboard_Click_Sound");

    //    if (effectClip != null)
    //    {
    //        soundManager.Play(effectClip, Sound.Effect, 1.0f);
    //        Debug.Log("Chatting sound is coming out!");
    //    }
    //    else
    //    {
    //        Debug.LogWarning("효과음을 찾을 수 없습니다: Sound/Keyboard_Click_Sound");
    //    }
    //}


    public void Start()
    {
        // 닉네임 입력 필드의 이벤트에 리스너 추가
        ChatInputField.onValueChanged.AddListener(OnChatChanged);
    }

    public void OnChatChanged(string Chat)
    {
        // 채팅이 입력될 때마다 효과음 재생
        AudioClip typingSoundClip = Resources.Load<AudioClip>("Sound/Keyboard_Click_Sound");
        soundManager.Play(typingSoundClip, Sound.Effect, 3.0f, 0.6f);
    }


}
