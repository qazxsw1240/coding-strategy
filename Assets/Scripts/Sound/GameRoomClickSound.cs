using System.Collections;

using UnityEngine;

namespace CodingStrategy.Sound
{
    public class GameRoomClickSound : MonoBehaviour
    {
        [SerializeField]
        private ISoundManager soundManager;

        // 게임룸씬에서 명령어 리스트 버튼 클릭 및 명령어 클릭 시 나오는 사운드
        public void CommmandClicked()
        {
            StartCoroutine(CommandClickedSound(0));
        }

        public IEnumerator CommandClickedSound(float delay)
        {
            yield return new WaitForSeconds(delay);

            soundManager = SoundManager.Instance;

            // 효과음을 불러오고 재생합니다.
            AudioClip effectClip = Resources.Load<AudioClip>("Sound/GameRoom_CommnadClicked_Sound");
            soundManager.Play(effectClip);
            Debug.Log("Command sound is comming out!");
        }

        // 게임룸씬에서 준비 버튼을 누를 시 나오는 사운드
        public void ReadyBtnClicked()
        {
            StartCoroutine(ReadyBtnClickedSound(0));
        }

        public IEnumerator ReadyBtnClickedSound(float delay)
        {
            yield return new WaitForSeconds(delay);

            soundManager = SoundManager.Instance;

            // 효과음을 불러오고 재생합니다.
            AudioClip effectClip = Resources.Load<AudioClip>("Sound/GameRoom_Ready");
            soundManager.Play(effectClip);
            Debug.Log("Ready button sound is comming out!");
        }

        // 게임룸씬에서 게임 시작 버튼을 누를 시 나오는 사운드
        public void GamestartBtnClicked()
        {
            StartCoroutine(GamestartBtnClickedSound(0));
        }

        public IEnumerator GamestartBtnClickedSound(float delay)
        {
            yield return new WaitForSeconds(delay);
            soundManager = SoundManager.Instance;
            AudioClip effectClip = Resources.Load<AudioClip>("Sound/GameRoom_GameStartSound");
            soundManager.Play(effectClip, SoundType.Effect, 1.0f, 0.5f);
        }

        // 게임룸씬에서 나가기 버튼 눌렀을 때 나오는 사운드
        public void QuitBtnClicked()
        {
            StartCoroutine(QuitBtnClickedSound(0));
        }

        public IEnumerator QuitBtnClickedSound(float delay)
        {
            yield return new WaitForSeconds(delay);

            soundManager = SoundManager.Instance;
            AudioClip effectClip = Resources.Load<AudioClip>("Sound/GameRoom_QuitBtn_Sound");
            soundManager.Play(effectClip);
        }
    }
}
