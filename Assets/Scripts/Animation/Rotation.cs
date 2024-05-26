using UnityEngine;


namespace CodingStrategy.Entities.Animations
{
    public class Rotation : MonoBehaviour
    {
        // 회전 속도를 조절할 변수
        public float rotationSpeed = 30f;

        // 매 프레임마다 호출되는 함수
        void Update()
        {
            // 오브젝트를 회전시킵니다.
            // Vector3.up은 y축을 기준으로 회전을 나타냅니다. 다른 축을 기준으로 회전하려면 변경할 수 있습니다.
            transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
        }
    }
}
