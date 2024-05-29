using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameSoundManager : MonoBehaviour
{
    private SoundManager soundManager;

    // PlayerInfo 또는 PlayerColor를 클릭했을 때 소리(RobotStatus 보일 때 소리)
    // RobotStatus 닫기 소리가 구현되지 않음.
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

    // BadSector 깔릴 때 소리
    public IEnumerator BadSectorSound(float delay)
    {
        yield return new WaitForSeconds(delay);

        soundManager = FindObjectOfType<SoundManager>();
        soundManager.Init();

        // 효과음을 불러오고 재생합니다.
        AudioClip effectClip = Resources.Load<AudioClip>("Sound/GameScene_BadSctor_Sound");
        soundManager.Play(effectClip, Sound.Effect, 1.0f);
        Debug.Log("Badsector sound is comming out!");
    }

    // 코인 먹을 때 소리
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

    // 공격당할 때 소리 GameScene_GetAttacked_Sound
    public IEnumerator GotAttackedSound(float dealy)
    {
        yield return new WaitForSeconds(dealy);

        soundManager = FindObjectOfType<SoundManager>();
        soundManager.Init();

        // 효과음을 불러오고 재생합니다.
        AudioClip effectClip = Resources.Load<AudioClip>("Sound/GameScene_GotAttacked_Sound");
        soundManager.Play(effectClip, Sound.Effect, 1.0f, 0.2f);
        Debug.Log("GetAttacked sound is comming out!");
    }

    // 코딩타임 카운트 다운 소리
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

    // 코딩타임 Bgm
    //public IEnumerator CodingTimeBgm(float delay)
    //{
    //    yield return new WaitForSeconds(delay);

    //    soundManager = FindObjectOfType<SoundManager>();
    //    soundManager.Init();

    //    // 효과음을 불러오고 재생합니다.
    //    AudioClip BgmClip = Resources.Load<AudioClip>("Sound/GameScene_CodingTimeCountdown_Sound");
    //    soundManager.Play(BgmClip, Sound.Bgm, 1.0f, 0.5f, 0.2f);
    //    Debug.Log("CodingTime Bgm is comming out!");
    //}


    // 캐릭터 걸을 때 사운드
    // 한 발자국 갈 때마다 나야하는 소리임.
    public IEnumerator CharacterWalkSound(float delay)
    {
        yield return new WaitForSeconds(delay);

        soundManager = FindObjectOfType<SoundManager>();
        soundManager.Init();

        // 효과음을 불러오고 재생합니다.
        AudioClip effectClip = Resources.Load<AudioClip>("Sound/GameScene_CharacterWalk_Sound");
        soundManager.Play(effectClip, Sound.Effect, 1.0f);
        Debug.Log("Character walk sound is comming out!");
    }

    // 코딩타임 명령어 배치 사운드
    // 


}
