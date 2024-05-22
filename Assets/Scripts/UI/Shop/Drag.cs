using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace CodingStrategy.UI.Shop
{
    public class Drag : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        [SerializeField] private Image _image;
        public Vector3 _oldPosition;

        // Start is called before the first frame update
        void Start()
        {
            _image = GetComponent<Image>();
            _oldPosition = _image.rectTransform.position;
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
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            transform.position = _oldPosition - new Vector3(Screen.width, 0, 0);
            _image.raycastTarget = true;
        }

        public void SetNewPosition(Vector3 newPosition)
        {
            _oldPosition = newPosition;
        }
    }
}
