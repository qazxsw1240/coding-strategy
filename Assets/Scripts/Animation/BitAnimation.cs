using System.Collections;

using DG.Tweening;

using UnityEngine;

namespace CodingStrategy.Animation
{
    public class BitAnimation : MonoBehaviour
    {
        public float rotationSpeed = 50f; // 얼만큼 회전
        public float moveSpeed = 0.6f; // 얼만큼 빠르게
        public float moveRange = 0.1f; // 얼만큼 이동

        public float fallDuration = 1f;
        public float fallDistance = 10f;
        public bool checkanimate;

        [SerializeField]
        private AudioSource musicsource;

        private Vector3 startPosition; // 오브젝트 위치 지점 저장

        private void Start()
        {
            StartCoroutine(AnimateItem());
        }

        private void Update()
        {
            if (!checkanimate)
            {
                return;
            }

            // Rotate around the z-axis at the specified speed
            transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);

            // Move up and down within the specified range at the specified speed
            float newY = startPosition.y + Mathf.Sin(Time.time * moveSpeed) * moveRange / 2;
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
        }

        public void GetBit()
        {
            rotationSpeed = 1000f;
            moveSpeed = 10f;
            moveRange = 2f;

            musicsource.Play();
            Destroy(gameObject, 0.47f);
        }

        private IEnumerator AnimateItem()
        {
            startPosition = transform.position; // 생성된 위치 저장
            musicsource.time = 0.3f; // 음악의 0.3초부터 시작s
            Sequence sequence = DOTween.Sequence();
            Vector3 endPosition = transform.position;
            transform.position = new Vector3(transform.position.x, transform.position.y + 10, transform.position.z);
            sequence.Insert(0, transform.DOMove(endPosition, fallDuration).SetEase(Ease.OutCubic));
            yield return sequence.WaitForCompletion();
            checkanimate = true;
        }
    }
}
