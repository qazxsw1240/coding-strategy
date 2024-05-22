using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace CodingStrategy.UI.GameRoom
{
    public class CommandInfo : MonoBehaviour
    {
        public TMP_Text commandName;
        public TMP_Text commandDetail;

        public void setCommandName(string commandname)
        {
            commandName.SetText(commandname);
        }

        public void setCommandDetail(string detail)
        {
            commandDetail.SetText(detail);
        }

        void Start() {}

        // Update is called once per frame
        void Update() {}
    }
}
