using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLobbyEnterBtnSound : MonoBehaviour
{
    private void Start()
    {
        // SoundManager 초기화는 Manager.Instance가 처리합니다.
        // 여기서는 별도로 초기화할 필요가 없습니다.
        if (Manager.Sound == null)
        {
            Debug.LogError("SoundManager가 초기화되지 않았습니다.");
        }
    }

    public void GameLobbyEnterBtnClicked()
    {
        // 효과음을 불러오고 재생합니다.
        AudioClip effectClip = Resources.Load<AudioClip>("Sound/GameRoom_GameStartSound");
        if (effectClip != null)
        {
            Manager.Sound.Play(effectClip, Sound.Effect, 1.0f, 0.5f);
            Debug.Log("방 참여 버튼 효과음이 재생됩니다!");
        }
        else
        {
            Debug.LogWarning("효과음을 찾을 수 없습니다: Sound/GameRoom_GameStartSound");
        }
    }
}
