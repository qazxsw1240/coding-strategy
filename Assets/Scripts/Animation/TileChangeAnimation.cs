using UnityEngine;
using UnityEngine.Serialization;

namespace CodingStrategy.Animation
{
    public class TileChangeAnimation : MonoBehaviour
    {
        [FormerlySerializedAs("Tile")]
        [SerializeField]
        // 애니메이션을 표현할 Tile입니다.
        private GameObject _tile;

        // 설치될 상태이상들입니다.
        public GameObject stack;
        public GameObject troijan;
        public GameObject warm;
        public GameObject malware;
        public GameObject spyware;
        public GameObject adware;
    }
}
