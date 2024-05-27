using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CodingStrategy.UI.GameLobby
{
    public class RotationLoadingBar : MonoBehaviour
    {
        public Image loadingImage; // 회전할 이미지를 할당
        public float rotationSpeed = 100f; // 이미지의 회전 속도

        void Update()
        {
            // 매 프레임마다 이미지를 회전시킵니다
            loadingImage.transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
        }
    }
}
