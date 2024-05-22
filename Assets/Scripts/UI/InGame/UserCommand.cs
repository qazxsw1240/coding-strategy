using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CodingStrategy.UI.InGame
{
    public class UserCommand : MonoBehaviour
    {
        public GameObject Command;

        public void setCommand()
        {
            Instantiate(Command);
        }

        // Start is called before the first frame update
        void Start() {}

        // Update is called once per frame
        void Update() {}
    }
}
