using UnityEngine;

namespace CodingStrategy.Sound
{
    public class GameLobbyEnterBtnSound : MonoBehaviour
    {
        private void Start()
        {
            if (SoundManager.Instance is null)
            {
                Debug.LogError("SoundManager가 초기화되지 않았습니다.");
            }
        }
    }
}
