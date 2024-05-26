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
        public string slotName;
        private UnityEvent<int, int> OnBuyCommandEvent;
        private UnityEvent<int> OnSellCommandEvent;
        private UnityEvent<int, int> OnChangeCommandEvent;

        public void OnDrop(PointerEventData eventData)
        {
            if (gameObject == eventData.pointerDrag) return;
            if (eventData.pointerDrag.name != slotName) return;

            Drag draggable = eventData.pointerDrag.GetComponent<Drag>();
            if (draggable == null) return;

            if (slotName == "ShopCommand")
            {
                Drag drag = SetDrag();
                name = slotName = "MyCommand";
                OnBuyCommandEvent.Invoke(draggable.GetIndex(), drag.GetIndex());
                //transform.GetChild(0).GetComponent<Image>().sprite = draggable.transform.GetChild(0).GetComponent<Image>().sprite;
                //draggable.SetVisible(false);
            }
            else if (slotName == "MyCommand")
            {
                if (name == "MyCommand")
                {
                    Drag drag = SetDrag();
                    OnChangeCommandEvent.Invoke(draggable.GetIndex(), drag.GetIndex());
                }
                else if (name == "ItemSelectInfo")
                {
                    OnSellCommandEvent.Invoke(draggable.GetIndex());
                }
            }
        }

        private Drag SetDrag()
        {
            Drag drag = transform.GetComponent<Drag>();
            if (drag == null)
            {
                drag = transform.AddComponent<Drag>();
            }
            drag.SetIndex(transform.GetSiblingIndex());
            return drag;
        }

        // Start is called before the first frame update
        void Start()
        {
            ShopUi shopUi = GameObject.Find("ShopUI").GetComponent<ShopUi>();
            OnBuyCommandEvent = shopUi.OnBuyCommandEvent;
            OnSellCommandEvent = shopUi.OnSellCommandEvent;
            OnChangeCommandEvent = shopUi.OnChangeCommandEvent;
        }

        // Update is called once per frame
        void Update() {}
    }
}
