using CodingStrategy.Sound;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace CodingStrategy.UI.GameRoom
{
    public class CommandInfoLocation : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [FormerlySerializedAs("command")]
        [SerializeField]
        private Transform _command;

        [FormerlySerializedAs("commandInfo")]
        [SerializeField]
        private GameObject _commandInfo;

        [SerializeField]
        private ISoundManager _soundManager;

        public void OnPointerDown(PointerEventData eventData)
        {
            _soundManager = SoundManager.Instance;
            // Effect sound를 불러오고 재생합니다.
            AudioClip effectClip = Resources.Load<AudioClip>("Sound/InfoCreateSound");
            _soundManager.Play(effectClip);
            Vector3 commandPos = _command.position;
            _commandInfo.SetActive(true);
            _commandInfo.transform.position = new Vector3(commandPos.x + 200f, commandPos.y - 100f, commandPos.z);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _commandInfo.SetActive(false);
        }
    }
}
