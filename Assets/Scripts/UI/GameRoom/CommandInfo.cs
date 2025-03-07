using TMPro;

using UnityEngine;

namespace CodingStrategy.UI.GameRoom
{
    public class CommandInfo : MonoBehaviour
    {
        public TMP_Text commandName;
        public TMP_Text commandDetail;

        public void SetCommandName(string commandName)
        {
            this.commandName.SetText(commandName);
        }

        public void SetCommandDetail(string detail)
        {
            commandDetail.SetText(detail);
        }
    }
}
