using System.Collections;

using UnityEngine;

namespace CodingStrategy.Sound
{
    public class InGameSoundManager : MonoBehaviour
    {
        private ISoundManager _soundManager;

        private void Awake()
        {
            _soundManager = SoundManager.Instance;
        }

        // PlayerInfo 또는 PlayerColor를 클릭했을 때 소리(RobotStatus 보일 때 소리)
        // RobotStatus 닫기 소리가 구현되지 않음.
        public void RobotStatus()
        {
            StartCoroutine(RobotStatusSound(0));
        }

        public IEnumerator RobotStatusSound(float delay)
        {
            yield return new WaitForSeconds(delay);
            // 효과음을 불러오고 재생합니다.
            AudioClip effectClip = Resources.Load<AudioClip>("Sound/GameScene_RobotStatus_Sound");
            _soundManager.Play(effectClip);
            Debug.Log("PlayerInfo or PlayerColor is clicked!");
        }

        // 코인 먹을 때 소리. 구현 해야함.
        public IEnumerator GetCoinSound(float delay)
        {
            yield return new WaitForSeconds(delay);
            // 효과음을 불러오고 재생합니다.
            AudioClip effectClip = Resources.Load<AudioClip>("Sound/GameScene_GetCoin_Sound");
            _soundManager.Play(effectClip);
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
            // 효과음을 불러오고 재생합니다.
            AudioClip effectClip = Resources.Load<AudioClip>("Sound/GameScene_CoinSpawn_Sound");
            _soundManager.Play(effectClip);
            Debug.Log("Coin spawn sound is comming out!");
        }

        // 코딩타임 카운트 다운 소리. 구현 해야함
        public void CodingTimeCountdown()
        {
            StartCoroutine(CodingTimeCountdownSound(0));
        }

        private IEnumerator CodingTimeCountdownSound(float delay)
        {
            yield return new WaitForSeconds(delay);
            AudioClip effectClip = Resources.Load<AudioClip>("Sound/GameScene_CodingTimeCountdown_Sound");
            _soundManager.Play(effectClip, SoundType.Effect, 1.0f, 0.5f);
        }
    }
}
