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
		private int _oldIndex;

		private UnityEvent<int, int> OnBuyCommandEvent;
        private UnityEvent<int> OnSellCommandEvent;
        private UnityEvent<int, int> OnChangeCommandEvent;

		public int GetIndex()
		{
			return _oldIndex;
		}

		public void OnDrop(PointerEventData eventData)
        {
            GameObject dragObject = eventData.pointerDrag;
            Drag drag = dragObject.GetComponent<Drag>();
            if (drag == null) return;

            //Debug.Log("OnDrop: " + drag.getParent() + " " + transform.parent.name);
            if (drag.getParent() == "ShopCommandList" && transform.parent.name == "MyCommandList")
            {
                OnBuyCommandEvent.Invoke(drag.GetIndex(), GetIndex());
                //Debug.Log("OnBuyCommandEvent " + drag.GetIndex() + " " + GetIndex());
            }
            else if (drag.getParent() == "MyCommandList" && transform.parent.name == "MyCommandList")
            {
				OnChangeCommandEvent.Invoke(drag.GetIndex(), GetIndex());
				//Debug.Log("OnChangeCommandEvent " + drag.GetIndex() + " " + GetIndex());
            }
            else if (drag.getParent() == "MyCommandList" && name == "ItemSelectInfo")
            {
                OnSellCommandEvent.Invoke(drag.GetIndex());
                //Debug.Log("OnSellCommandEvent " + drag.GetIndex());
            }
        }

        // Start is called before the first frame update
        void Start()
        {
			_oldIndex = transform.GetSiblingIndex();
			ShopUi shopUi = GameObject.Find("ShopUI").GetComponent<ShopUi>();
            OnBuyCommandEvent = shopUi.OnBuyCommandEvent;
            OnSellCommandEvent = shopUi.OnSellCommandEvent;
            OnChangeCommandEvent = shopUi.OnChangeCommandEvent;
        }

        // Update is called once per frame
        void Update() {}
    }
}
