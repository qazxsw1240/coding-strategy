using UnityEngine;
using UnityEngine.UI;

namespace CodingStrategy.UI.Shop
{
    public class SlideBackground : MonoBehaviour
    {
        [SerializeField] private ScrollRect scrollRect;
        [SerializeField] private Image image;

        public void Start()
        {
            image = GetComponent<Image>();
        }

        public void Update()
        {
            Color color = image.color;
            color.a = 0.7f * scrollRect.horizontalNormalizedPosition;
            image.color = color;
        }
    }
}
