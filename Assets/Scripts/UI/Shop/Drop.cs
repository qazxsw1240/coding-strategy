using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


namespace CodingStrategy.UI.Shop
{
    public class Drop : MonoBehaviour, IDropHandler
    {
        public string slotName;

        public void OnDrop(PointerEventData eventData)
        {
            if (eventData.pointerDrag.transform.name == slotName)
            {
                Drag draggable = eventData.pointerDrag.GetComponent<Drag>();
                if (draggable != null)
                {
                    draggable._oldPosition = eventData.position + new Vector2(Screen.width, 0);
                }
            }
        }

        // Start is called before the first frame update
        void Start() {}

        // Update is called once per frame
        void Update() {}
    }
}
