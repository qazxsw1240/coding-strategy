using TMPro;

using UnityEngine;

namespace CodingStrategy.UI.GameRoom
{
    public class PlayerInfo : MonoBehaviour
    {
        public TMP_Text Name;
        public TMP_Text ReadyState;

        public void SetName(string name)
        {
            Name.SetText(name);
        }

        public void SetReady(bool ready)
        {
            if (ready)
            {
                ReadyState.SetText("준비 완료");
            }
            else
            {
                ReadyState.SetText("준비 중");
            }
        }
    }
}
