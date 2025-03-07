using UnityEngine;
using UnityEngine.UI;

namespace CodingStrategy.UI.GameScene
{
    public class DestroyOnClick : MonoBehaviour
    {
        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(() => Destroy(gameObject));
        }
    }
}
