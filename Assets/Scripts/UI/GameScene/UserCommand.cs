using UnityEngine;

namespace CodingStrategy.UI.GameScene
{
    public class UserCommand : MonoBehaviour
    {
        public GameObject command;

        public void SetCommand()
        {
            Instantiate(command);
        }
    }
}
