using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CodingStrategy.UI.GameScene
{
    public class QuitButtonManager : MonoBehaviour
    {
        public Button gameTurnButton; // Gameturn Button
        public Button gameRoomQuitButton; // GameroomQuit Button
        public UnityEvent OnQuitButtonClick;

        private void Start()
        {
            gameRoomQuitButton.gameObject.SetActive(false);
            gameTurnButton.onClick.AddListener(() => { gameRoomQuitButton.gameObject.SetActive(true); });
            gameRoomQuitButton.onClick.AddListener(
                () =>
                {
                    OnQuitButtonClick.Invoke();
                    gameRoomQuitButton.gameObject.SetActive(false);
                });
        }
    }
}
