using TMPro;

using UnityEngine;

namespace CodingStrategy.UI.GameScene
{
    public class GameTurn : MonoBehaviour
    {
        public TMP_Text gameTurn;

        // Start is called before the first frame update
        private void Start() {}

        // Update is called once per frame
        private void Update() {}

        public void SetTurn(int turn)
        {
            gameTurn.text = turn.ToString();
        }
    }
}
