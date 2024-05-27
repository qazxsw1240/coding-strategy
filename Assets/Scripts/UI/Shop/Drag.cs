using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace CodingStrategy.UI.Shop
{
    public class Drag : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        [SerializeField] private Image _image;
        private Transform _oldParent;
        private int _oldIndex;
        private Transform alwaysOntop;
        private Transform _tmpObject;

        public int GetIndex()
        {
            return _oldIndex;
        }

        public string getParent()
        {
            return _oldParent.name;
        }

        // Start is called before the first frame update
        void Start()
        {
            _image = GetComponent<Image>();
            alwaysOntop = GameObject.Find("AlwaysOnTop").transform;
		}

        // Update is called once per frame
        void Update() { }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = eventData.position;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _image.raycastTarget = false;
			_oldParent = transform.parent;
			_oldIndex = transform.GetSiblingIndex();
			SetTmp();
            transform.SetParent(alwaysOntop);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            ResetPosition();
        }

        public void ResetPosition()
        {
            if (_tmpObject != null)
            {
                Destroy(_tmpObject.gameObject);
            }
            GetComponent<Image>().raycastTarget = true;
            transform.SetParent(_oldParent);
            transform.SetSiblingIndex(_oldIndex);
        }

        public void SetTmp()
        {
            if (_tmpObject == null)
            {
                _tmpObject = new GameObject().AddComponent<RectTransform>();
                _tmpObject.SetParent(transform.parent);
                _tmpObject.SetSiblingIndex(_oldIndex);
            }
        }

        public bool GetVisible()
        {
            return _image.enabled;
        }

        public void SetVisible(bool visiblility)
        {
            _image.enabled = visiblility;
            transform.GetChild(0).GetComponent<Image>().enabled = visiblility;
        }
    }
}
