using CodingStrategy.Sound;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CodingStrategy.UI.Shop
{
    public class Drag : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        [SerializeField]
        private Image _image;

        private Transform _alwaysOntop;

        private int _oldIndex;
        private Transform _oldParent;

        private ISoundManager _soundManager; // soundManager
        private Transform _tmpObject;

        // Start is called before the first frame update
        private void Start()
        {
            if (transform.parent.name != "MyCommandList" && transform.parent.name != "ShopCommandList")
            {
                Destroy(gameObject.GetComponent<Drag>());
                return;
            }

            _image = GetComponent<Image>();
            _alwaysOntop = GameObject.Find("AlwaysOnTop").transform;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            // Begin drag sound
            _soundManager = SoundManager.Instance;
            AudioClip effectClip = Resources.Load<AudioClip>("Sound/Shop_CommandClick_Sound");
            _soundManager.Play(effectClip);
            Debug.Log("Command clicked sound is comming out!");

            _image.raycastTarget = false;
            _oldParent = transform.parent;
            _oldIndex = transform.GetSiblingIndex();
            SetTmp();
            transform.SetParent(_alwaysOntop);
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            ResetPosition();
            // End drag sound
            _soundManager = SoundManager.Instance;
            AudioClip effectClip = Resources.Load<AudioClip>("Sound/GameScene_CommandPutdown_Sound");
            _soundManager.Play(effectClip);
            Debug.Log("Commnad put down sound is comming out!");
        }

        public int GetIndex()
        {
            return _oldIndex;
        }

        public string GetParent()
        {
            return _oldParent.name;
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
    }
}
