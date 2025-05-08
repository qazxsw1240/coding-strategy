using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace CodingStrategy.Photon
{
    public class ChatRenderer : MonoBehaviour
    {
        [SerializeField]
        private TMP_InputField _chatField;

        [SerializeField]
        private TMP_Text _chatDisplay;

        [SerializeField]
        private Button _sendButton;

        private ChatManager _chatManager;

        private void Awake()
        {
            if (!_chatField)
            {
                throw new UnassignedReferenceException("Cannot find chat field");
            }
            if (!_chatDisplay)
            {
                throw new UnassignedReferenceException("Cannot find chat display");
            }
            _chatManager = GetComponent<ChatManager>();
            if (!_chatManager)
            {
                throw new UnassignedReferenceException("Cannot find chat manager");
            }
        }

        private void Start()
        {
            _sendButton.onClick.AddListener(OnSendButtonPressed);
            _chatManager.OnChatMessageReceived += OnChatMessageReceived;
        }

        private void OnDestroy()
        {
            _sendButton.onClick.RemoveListener(OnSendButtonPressed);
            _chatManager.OnChatMessageReceived -= OnChatMessageReceived;
        }

        private void OnSendButtonPressed()
        {
            _chatManager.Send(_chatField.text);
            _chatField.text = "";
        }

        private void OnChatMessageReceived(ChatMessage message)
        {
            _chatDisplay.text += "\n" + $"{message.Sender}: {message.Message}";
        }
    }
}
