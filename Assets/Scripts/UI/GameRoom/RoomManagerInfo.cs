using TMPro;

using UnityEngine;

namespace CodingStrategy.UI.GameRoom
{
    public class RoomManagerInfo : MonoBehaviour
    {
        public TMP_Text Name;

        private void Start()
        {
            setName("ABC");
        }

        // Update is called once per frame
        private void Update() {}
        // is called before the first frame update

        public void setName(string name)
        {
            Name.SetText(name);
        }
    }
}
