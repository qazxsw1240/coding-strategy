using TMPro;

using UnityEngine;

namespace CodingStrategy.UI.GameLobby
{
    public class ModeInfo : MonoBehaviour
    {
        public TMP_Text modeTitle;
        public TMP_Text modeDescription;

        public void SetMode(string name, string detail)
        {
            modeTitle.SetText(name);
            modeDescription.SetText(detail);
        }
    }
}
