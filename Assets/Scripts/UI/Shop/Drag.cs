using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace CodingStrategy.UI.Shop
{
    public class Drag : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        [SerializeField] private Image _image;
        private Vector3 _oldPosition;
        private Transform _oldParent;
        private int _oldIndex;
        private Transform alwaysOntop;
        private Transform _tmpObject;

        // Start is called before the first frame update
        void Start()
        {
            _image = GetComponent<Image>();
            _oldPosition = _image.rectTransform.position;
            _oldParent = transform.parent;
            _oldIndex = transform.GetSiblingIndex();
			alwaysOntop = GameObject.Find("AlwaysOnTop").transform;
        }

        // Update is called once per frame
        void Update() {}

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = eventData.position;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _image.raycastTarget = false;
            _tmpObject = new GameObject().AddComponent<RectTransform>();
            _tmpObject.SetParent(transform.parent);
            _tmpObject.SetSiblingIndex(_oldIndex);
            transform.SetParent(alwaysOntop);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            ResetPosition();
            Debug.Log("OnEndDrag " + name + " " + eventData.pointerDrag.name);
            _image.raycastTarget = true;
			transform.SetParent(_oldParent);
            transform.SetSiblingIndex(_oldIndex);
            Destroy(_tmpObject.gameObject);
		}

        public void ResetPosition()
        {
			transform.position = _oldPosition - new Vector3(Screen.width * GameObject.Find("Shop Scroll View").GetComponent<ScrollRect>().horizontalNormalizedPosition, 0, 0);
		}
    }
}
