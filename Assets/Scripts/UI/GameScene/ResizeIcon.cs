using UnityEngine;
using UnityEngine.UI;

namespace CodingStrategy.UI.GameScene
{
    public class ResizeIcon : MonoBehaviour
    {
        private static readonly Vector2 DefaultScale = new Vector2(200f, 200f);

        private void Start()
        {
            Transform parentTransform = transform.parent;
            GridLayoutGroup gridLayoutGroup = parentTransform.GetComponent<GridLayoutGroup>();
            foreach (Transform child in transform)
            {
                RectTransform rectTransform = child.GetComponent<RectTransform>();
                rectTransform.sizeDelta *= gridLayoutGroup.cellSize / DefaultScale;
            }
        }
    }
}
