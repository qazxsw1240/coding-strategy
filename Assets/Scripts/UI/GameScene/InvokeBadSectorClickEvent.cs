using UnityEngine;

namespace CodingStrategy.UI.GameScene
{
    public class InvokeBadSectorClickEvent : MonoBehaviour
    {
        private void OnMouseDown()
        {
            GameObject activeChild = null;
            foreach (Transform child in transform)
            {
                if (!child.gameObject.activeSelf)
                {
                    continue;
                }
                activeChild = child.gameObject;
                break;
            }
            if (!activeChild)
            {
                return;
            }
            BadSectorClickEvent badSectorClickEvent =
                GameObject.Find("AlwaysOnTop").GetComponent<BadSectorClickEvent>();
            badSectorClickEvent.OnBadSectorClickEvent.Invoke(activeChild.name);
            badSectorClickEvent.setBadSectorDetail = GetComponent<SetBadSectorDetail>();
        }
    }
}
