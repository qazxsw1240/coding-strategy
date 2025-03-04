using CodingStrategy.UI.InGame;

using UnityEngine;
using UnityEngine.Events;

namespace CodingStrategy.UI.GameScene
{
    public class CommandDetailEvent : MonoBehaviour
    {
        public UnityEvent<string> OnCommandClickEvent;
        public SetCommandDetail setCommandDetail;

        // Start is called before the first frame update
        private void Start() {}

        // Update is called once per frame
        private void Update() {}
    }
}
