using UnityEngine;
using UnityEngine.UI;

namespace CodingStrategy.Sound
{
    public class ChatSoundManager : MonoBehaviour
    {
        public InputField ChatInputField;
        private SoundManager soundManager;

        public void Start()
        {
            // 닉네임 입력 필드의 이벤트에 리스너 추가
            ChatInputField.onValueChanged.AddListener(OnChatChanged);
        }

        public void OnChatChanged(string chat)
        {
            AudioClip typingSoundClip = Resources.Load<AudioClip>("Sound/Keyboard_Click_Sound");
            soundManager.Play(typingSoundClip, SoundType.Effect, 3.0f, 0.6f);
        }
    }
}
