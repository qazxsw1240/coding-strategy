using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CodingStrategy.UI.GameLobby
{
    public class RotationLoadingBar : MonoBehaviour
    {
        public float rotationSpeed = 100f; // 회전 속도

        void Update()
        {
            // 매 프레임마다 UI 요소를 회전시킵니다
            transform.Rotate(0, 0, rotationSpeed * Time.deltaTime); // Z 축을 중심으로 회전
        }
    }
}
