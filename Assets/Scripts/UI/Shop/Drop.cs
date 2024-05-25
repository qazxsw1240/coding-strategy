using CodingStrategy.UI.InGame;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CodingStrategy.UI.Shop
{
    public class Drop : MonoBehaviour, IDropHandler
    {
        public string slotName;
        private Sprite emptySprite;

        public void OnDrop(PointerEventData eventData)
        {
            if (eventData.pointerDrag.name == slotName)
            {
                if (slotName == "ShopItem")
                {
                    Drag draggable = eventData.pointerDrag.GetComponent<Drag>();
                    if (draggable == null) return;
                    Debug.Log("Ondrop Buy index " + draggable.transform.GetSiblingIndex() + " to " + transform.GetSiblingIndex());
                    transform.GetChild(0).GetComponent<Image>().sprite = draggable.transform.GetChild(0).GetComponent<Image>().sprite;
                    transform.name = "SelectedItem";
                    transform.AddComponent<Drag>();
                    //Destroy(GetComponent<Drop>());
                    slotName = "SelectedItem";
                    //draggable.ResetPosition();
                }
                else if (slotName == "SelectedItem")
                {
					Drag draggable = eventData.pointerDrag.GetComponent<Drag>();
                    if (draggable == null) return;
					Debug.Log("Ondrop Sell index " + draggable.transform.GetSiblingIndex());
				}
			}
        }

        // Start is called before the first frame update
        void Start()
        {
            ;
        }

        // Update is called once per frame
        void Update() {}
    }
}
