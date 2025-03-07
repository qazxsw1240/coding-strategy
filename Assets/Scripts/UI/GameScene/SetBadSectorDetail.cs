using TMPro;

using UnityEngine;

namespace CodingStrategy.UI.GameScene
{
    public class SetBadSectorDetail : MonoBehaviour
    {
        public TMP_Text Name;
        public TMP_Text Description;

        public void SetName(string badSectorName)
        {
            Name.text = badSectorName;
        }

        public void SetDescription(string badSectorDescription)
        {
            Description.text = badSectorDescription;
        }
    }
}
