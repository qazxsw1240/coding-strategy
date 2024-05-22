using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


namespace CodingStrategy.UI.GameLobby
{
    public class ModeInfo : MonoBehaviour
    {
        public TMP_Text modeTitle;
        public TMP_Text modeDescription;

        public void setMode(string name, string detail)
        {
            modeTitle.SetText(name);
            modeDescription.SetText(detail);
        }

        // Start is called before the first frame update
        void Start() {}

        // Update is called once per frame
        void Update() {}
    }
}
