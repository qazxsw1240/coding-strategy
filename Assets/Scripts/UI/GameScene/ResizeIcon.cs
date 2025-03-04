using UnityEngine;
using UnityEngine.UI;

namespace CodingStrategy.UI.GameScene
{
    public class ResizeIcon : MonoBehaviour
    {
        // Start is called before the first frame update
        private void Start()
        {
            foreach (Transform child in transform)
            {
                child.GetComponent<RectTransform>().sizeDelta *=
                    transform.parent.GetComponent<GridLayoutGroup>().cellSize / new Vector2(200.0f, 200.0f);
            }
        }
    }
}
