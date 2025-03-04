using CodingStrategy.Sound;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CodingStrategy.UI.Shop
{
    public class ShopScroll : MonoBehaviour, IBeginDragHandler, IEndDragHandler
    {
        [SerializeField]
        private bool _isScrolling;

        private ScrollRect scrollRect;

        public bool Scrolling => _isScrolling;

        private void Start()
        {
            scrollRect = GetComponent<ScrollRect>();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            ISoundManager soundManager = SoundManager.Instance;
            AudioClip effectClip = Resources.Load<AudioClip>("Sound/Shop_DragBtnClick_Sound");
            soundManager.Play(effectClip);
            _isScrolling = true;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _isScrolling = false;
        }
    }
}
