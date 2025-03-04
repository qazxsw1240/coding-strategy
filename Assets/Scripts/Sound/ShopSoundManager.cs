using System.Collections;

using UnityEngine;

namespace CodingStrategy.Sound
{
    public class ShopSoundManager : MonoBehaviour
    {
        [SerializeReference]
        private ISoundManager _soundManager;

        // 상점 삼각형 버튼 클릭 할 때 사운드
        public void ShopDragBtnClicked()
        {
            StartCoroutine(ShopDragBtnClickedSound(0));
        }

        public IEnumerator ShopDragBtnClickedSound(float delay)
        {
            yield return new WaitForSeconds(delay);
            _soundManager = SoundManager.Instance;
            AudioClip effectClip = Resources.Load<AudioClip>("Sound/Shop_DragBtnClick_Sound");
            _soundManager.Play(effectClip);
        }

        // 상점에서 리롤 사운드
        public void RerollBtnClicked()
        {
            StartCoroutine(RerollBtnClickedSound(0));
        }

        public IEnumerator RerollBtnClickedSound(float delay)
        {
            yield return new WaitForSeconds(delay);

            _soundManager = SoundManager.Instance;

            // 효과음을 불러오고 재생합니다.
            AudioClip effectClip = Resources.Load<AudioClip>("Sound/Shop_Reroll_Sound");
            _soundManager.Play(effectClip);
        }

        // 상점에서 레벨업 사운드
        public void LevelUpClicked()
        {
            StartCoroutine(LevelUpButtonClickedSound(0));
        }

        public IEnumerator LevelUpButtonClickedSound(float delay)
        {
            yield return new WaitForSeconds(delay);
            _soundManager = SoundManager.Instance;
            AudioClip effectClip = Resources.Load<AudioClip>("Sound/Shop_Levelup_Sound");
            _soundManager.Play(effectClip);
        }
    }
}
