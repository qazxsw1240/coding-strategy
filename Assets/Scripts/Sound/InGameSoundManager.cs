using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameSoundManager : MonoBehaviour
{
    private SoundManager soundManager;

    // PlayerInfo 또는 PlayerColor를 클릭했을 때 소리(RobotStatus 보일 때 소리)
    // RobotStatus 닫기 소리가 구현되지 않음.
    public void RobotStatus()
    {
        StartCoroutine(RobotStatusSound(0));
    }
    public IEnumerator RobotStatusSound(float delay)
    {
        yield return new WaitForSeconds(delay);

        soundManager = FindObjectOfType<SoundManager>();
        soundManager.Init();

        // 효과음을 불러오고 재생합니다.
        AudioClip effectClip = Resources.Load<AudioClip>("Sound/GameScene_RobotStatus_Sound");
        soundManager.Play(effectClip, Sound.Effect, 1.0f);
        Debug.Log("PlayerInfo or PlayerColor is clicked!");
    }

    // 코인 먹을 때 소리. 구현 해야함.
    public IEnumerator GetCoinSound(float delay)
    {
        yield return new WaitForSeconds(delay);

        soundManager = FindObjectOfType<SoundManager>();
        soundManager.Init();

        // 효과음을 불러오고 재생합니다.
        AudioClip effectClip = Resources.Load<AudioClip>("Sound/GameScene_GetCoin_Sound");
        soundManager.Play(effectClip, Sound.Effect, 1.0f);
        Debug.Log("Coin get sound is comming out!");
    }

    // 코인 생성될 때 소리. 구현하면 좋을 것 같음.
    public void CoinSpawn()
    {
        StartCoroutine(CoinSpawnSound(0));
    }
    public IEnumerator CoinSpawnSound(float delay)
    {
        yield return new WaitForSeconds(delay);

        soundManager = FindObjectOfType<SoundManager>();
        soundManager.Init();

        // 효과음을 불러오고 재생합니다.
        AudioClip effectClip = Resources.Load<AudioClip>("Sound/GameScene_CoinSpawn_Sound");
        soundManager.Play(effectClip, Sound.Effect, 1.0f);
        Debug.Log("Coin spawn sound is comming out!");
    }

    // 코딩타임 카운트 다운 소리. 구현 해야함
    public void CodingTimeCountdown()
    {
        StartCoroutine(CodingTimeCountdownSound(0));
    }
    public IEnumerator CodingTimeCountdownSound(float dealy)
    {
        yield return new WaitForSeconds(dealy);

        soundManager = FindObjectOfType<SoundManager>();
        soundManager.Init();

        // 효과음을 불러오고 재생합니다.
        AudioClip effectClip = Resources.Load<AudioClip>("Sound/GameScene_CodingTimeCountdown_Sound");
        soundManager.Play(effectClip, Sound.Effect, 1.0f, 0.5f);
        Debug.Log("CodingTime countdown sound is comming out!");
    }

    /*
    // 턴 바뀔 때 사운드. 없는게 나을 것 같음.
    public IEnumerator GameTurnSound(float delay)
    {
        yield return new WaitForSeconds(delay);

        soundManager = FindObjectOfType<SoundManager>();
        soundManager.Init();

        // 효과음을 불러오고 재생합니다.
        AudioClip effectClip = Resources.Load<AudioClip>("Sound/GameScene_GameTurnChanged_Sound");
        soundManager.Play(effectClip, Sound.Effect, 1.0f);
        Debug.Log("Gameturn sound is comming out!");

    }*/



    // 코딩타임 명령어 배치 사운드??
    // 


}
