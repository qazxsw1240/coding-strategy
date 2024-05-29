using CodingStrategy.UI.InGame;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CodingStrategy.UI.Shop
{
    public class Drop : MonoBehaviour, IDropHandler
    {
        private UnityEvent<int, int> OnBuyCommandEvent;
        private UnityEvent<int> OnSellCommandEvent;
        private UnityEvent<int, int> OnChangeCommandEvent;

        public int GetIndex()
        {
            return transform.GetSiblingIndex();
        }

        public void OnDrop(PointerEventData eventData)
        {
            GameObject dragObject = eventData.pointerDrag;
            Drag drag = dragObject.GetComponent<Drag>();
            if (drag == null) return;

            //Debug.Log("OnDrop: " + drag.getParent() + " " + transform.parent.name);
            if (drag.getParent() == "ShopCommandList" && transform.parent.name == "MyCommandList")
            {
                StartCoroutine(OnBuyCoroutine(drag.GetIndex(), GetIndex()));
            }
            else if (drag.getParent() == "MyCommandList" && name == "ItemSelectInfo")
            {
                StartCoroutine(OnSellCoroutine(drag.GetIndex()));
            }
            else if (drag.getParent() == "MyCommandList" && transform.parent.name == "MyCommandList")
            {
                StartCoroutine(OnChangeCoroutine(drag.GetIndex(), GetIndex()));
            }
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

        // Start is called before the first frame update
        void Start()
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

        // Update is called once per frame
        void Update() { }
    }
}
