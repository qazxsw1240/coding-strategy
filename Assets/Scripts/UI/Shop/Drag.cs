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

        private SoundManager soundManager; // 사운드

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
            if (transform.parent.name != "MyCommandList" && transform.parent.name != "ShopCommandList")
            {
                Destroy(gameObject.GetComponent<Drag>());
                return;
            }
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
            // 드래그 시작 사운드
            soundManager = FindObjectOfType<SoundManager>();
            soundManager.Init();
            AudioClip effectClip = Resources.Load<AudioClip>("Sound/Shop_CommandClick_Sound");
            soundManager.Play(effectClip, Sound.Effect, 1.0f);
            Debug.Log("Command clicked sound is comming out!");

            _image.raycastTarget = false;
			_oldParent = transform.parent;
			_oldIndex = transform.GetSiblingIndex();
			SetTmp();
            transform.SetParent(alwaysOntop);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            ResetPosition();
            // 명령어 배치 사운드
            soundManager = FindObjectOfType<SoundManager>();
            soundManager.Init();
            AudioClip effectClip = Resources.Load<AudioClip>("Sound/GameScene_CommandPutdown_Sound");
            soundManager.Play(effectClip, Sound.Effect, 1.0f);
            Debug.Log("Commnad put down sound is comming out!");
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
