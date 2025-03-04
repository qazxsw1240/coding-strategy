using System.Collections;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace CodingStrategy.UI.Shop
{
    public class Drop : MonoBehaviour, IDropHandler
    {
        private UnityEvent<int, int> OnBuyCommandEvent;
        private UnityEvent<int, int> OnChangeCommandEvent;
        private UnityEvent<int> OnSellCommandEvent;

        // Start is called before the first frame update
        private void Start()
        {
            if (name != "ItemSelectInfo" && transform.parent.name != "MyCommandList")
            {
                Destroy(gameObject.GetComponent<Drop>());
                return;
            }

            ShopUi shopUi = GameObject.Find("ShopUI").GetComponent<ShopUi>();
            OnBuyCommandEvent = shopUi.OnBuyCommandEvent;
            OnSellCommandEvent = shopUi.OnSellCommandEvent;
            OnChangeCommandEvent = shopUi.OnChangeCommandEvent;
        }

        public void OnDrop(PointerEventData eventData)
        {
            GameObject dragObject = eventData.pointerDrag;
            Drag drag = dragObject.GetComponent<Drag>();
            if (drag == null)
            {
                return;
            }

            //Debug.Log("OnDrop: " + drag.getParent() + " " + transform.parent.name);
            if (drag.GetParent() == "ShopCommandList" && transform.parent.name == "MyCommandList")
            {
                StartCoroutine(OnBuyCoroutine(drag.GetIndex(), GetIndex()));
            }
            else if (drag.GetParent() == "MyCommandList" && name == "ItemSelectInfo")
            {
                StartCoroutine(OnSellCoroutine(drag.GetIndex()));
            }
            else if (drag.GetParent() == "MyCommandList" && transform.parent.name == "MyCommandList")
            {
                StartCoroutine(OnChangeCoroutine(drag.GetIndex(), GetIndex()));
            }
        }

        public int GetIndex()
        {
            return transform.GetSiblingIndex();
        }

        public IEnumerator OnBuyCoroutine(int drag, int drop)
        {
            yield return new WaitForEndOfFrame();
            OnBuyCommandEvent.Invoke(drag, drop);
            //Debug.Log("OnBuyCommandEvent " + drag + " " + drop);
        }

        public IEnumerator OnSellCoroutine(int drag)
        {
            yield return new WaitForEndOfFrame();
            OnSellCommandEvent.Invoke(drag);
            //Debug.Log("OnSellCommandEvent " + drag);
        }

        public IEnumerator OnChangeCoroutine(int drag, int drop)
        {
            yield return new WaitForEndOfFrame();
            OnChangeCommandEvent.Invoke(drag, drop);
            //Debug.Log("OnChangeCommandEvent " + drag + " " + drop);
        }
    }
}
