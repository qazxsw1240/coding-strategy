using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopSoundManager : MonoBehaviour
{

    private SoundManager soundManager;

    // 상점 삼각형 버튼 클릭 할 때 사운드
    public void ShopDragBtnClicked()
    {
        StartCoroutine(ShopDragBtnClickedSound(0));
    }
    public IEnumerator ShopDragBtnClickedSound(float delay)
    {
        yield return new WaitForSeconds(delay);

        soundManager = FindObjectOfType<SoundManager>();
        soundManager.Init();

        // 효과음을 불러오고 재생합니다.
        AudioClip effectClip = Resources.Load<AudioClip>("Sound/Shop_DragBtnClick_Sound");
        soundManager.Play(effectClip, Sound.Effect, 1.0f);
        Debug.Log("Drag sound is comming out!");
    }

    // 상점에서 리롤 사운드
    public void RerollBtnClicked()
    {
        StartCoroutine(RerollBtnClickedSound(0));
    }
    public IEnumerator RerollBtnClickedSound(float delay)
    {
        yield return new WaitForSeconds(delay);

        soundManager = FindObjectOfType<SoundManager>();
        soundManager.Init();

        // 효과음을 불러오고 재생합니다.
        AudioClip effectClip = Resources.Load<AudioClip>("Sound/Shop_Reroll_Sound");
        soundManager.Play(effectClip, Sound.Effect, 1.0f);
        Debug.Log("Reroll sound is comming out!");
    }

    // 상점에서 레벨업 사운드
    public void LevelupClicked()
    {
        StartCoroutine(LevelupBtnClickedSound(0));
    }
    public IEnumerator LevelupBtnClickedSound(float delay)
    {
        yield return new WaitForSeconds(delay);

        soundManager = FindObjectOfType<SoundManager>();
        soundManager.Init();

        // 효과음을 불러오고 재생합니다.
        AudioClip effectClip = Resources.Load<AudioClip>("Sound/Shop_Levelup_Sound");
        soundManager.Play(effectClip, Sound.Effect, 1.0f);
        Debug.Log("Levelup sound is comming out!");
    }

    /*
    // 상점에서 커멘드 옮기기 위해 클릭할 때 사운드
    public void CommandClicked()
    {
        StartCoroutine(CommandClickedSound(0));
    }
    public IEnumerator CommandClickedSound(float delay)
    {
        yield return new WaitForSeconds(delay);

        soundManager = FindObjectOfType<SoundManager>();
        soundManager.Init();

        // 효과음을 불러오고 재생합니다.
        AudioClip effectClip = Resources.Load<AudioClip>("Sound/Shop_CommandClick_Sound");
        soundManager.Play(effectClip, Sound.Effect, 1.0f);
        Debug.Log("CommnadClickedSound is comming out!");
    }*/

}
