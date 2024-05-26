using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopSoundManager : MonoBehaviour
{

    private SoundManager soundManager;

    public void ShopDragBtnClicked()
    {
        soundManager = FindObjectOfType<SoundManager>();
        soundManager.Init();

        // 효과음을 불러오고 재생합니다.
        AudioClip effectClip = Resources.Load<AudioClip>("Sound/GameLobby_UI_ClickSound");
        soundManager.Play(effectClip, Sound.Effect, 1.0f);
        Debug.Log("Drag sound is comming out!");
    }

    public void RerollBtnClicked()
    {
        soundManager = FindObjectOfType<SoundManager>();
        soundManager.Init();

        // 효과음을 불러오고 재생합니다.
        AudioClip effectClip = Resources.Load<AudioClip>("Sound/GameRoom_Ready");
        soundManager.Play(effectClip, Sound.Effect, 1.0f);
        Debug.Log("Reroll sound is comming out!");
    }

    public void LevelupBtnClicked()
    {
        soundManager = FindObjectOfType<SoundManager>();
        soundManager.Init();

        // 효과음을 불러오고 재생합니다.
        AudioClip effectClip = Resources.Load<AudioClip>("Sound/GameRoom_GameStartSound");
        soundManager.Play(effectClip, Sound.Effect, 1.0f);
        Debug.Log("Levelup sound is comming out!");
    }

    public void CommnadClickedSound()
    {
        soundManager = FindObjectOfType<SoundManager>();
        soundManager.Init();

        // 효과음을 불러오고 재생합니다.
        AudioClip effectClip = Resources.Load<AudioClip>("Sound/GameRoom_GameStartSound");
        soundManager.Play(effectClip, Sound.Effect, 1.0f);
        Debug.Log("CommnadClickedSound is comming out!");
    }
}
