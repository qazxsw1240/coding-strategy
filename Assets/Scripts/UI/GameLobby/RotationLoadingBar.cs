using UnityEngine;
using UnityEngine.UI;

namespace CodingStrategy.UI.GameLobby
{
    public class RotationLoadingBar : MonoBehaviour
    {
        public Image loadingImage;
        public float rotationSpeed = 100f;

        private void Update()
        {
            loadingImage.transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
        }
    }
}
