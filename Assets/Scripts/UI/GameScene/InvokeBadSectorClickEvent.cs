using CodingStrategy.UI.GameScene;

using UnityEngine;

namespace CodingStrategy.UI.InGame
{
    public class InvokeBadSectorClickEvent : MonoBehaviour
    {
        private void OnMouseDown()
        {
            GameObject activeChild = null;
            foreach (Transform child in transform)
            {
                if (child.gameObject.activeSelf)
                {
                    activeChild = child.gameObject;
                    break;
                }
            }
            BadSectorClickEvent badSectorClickEvent =
                GameObject.Find("AlwaysOnTop").GetComponent<BadSectorClickEvent>();
            badSectorClickEvent.OnBadSectorClickEvent.Invoke(activeChild.name);
            badSectorClickEvent.setBadSectorDetail = GetComponent<SetBadSectorDetail>();
        }
    }
}
