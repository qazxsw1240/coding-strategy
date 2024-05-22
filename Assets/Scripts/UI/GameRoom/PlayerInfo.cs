using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace CodingStrategy.UI.GameRoom
{
    public class PlayerInfo : MonoBehaviour
    {
        public TMP_Text Name;
        public TMP_Text ReadyState;

        public void setName(string name)
        {
            Name.SetText(name);
        }

        public void setReady(bool ready)
        {
            if (ready == true)
            {
                ReadyState.SetText("준비 완료");
            }
            else
            {
                ReadyState.SetText("준비 중");
            }
        }

        // Start is called before the first frame update
        void Start() {}

        // Update is called once per frame
        void Update() {}
    }
}
