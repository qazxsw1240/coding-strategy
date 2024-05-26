using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace CodingStrategy.Entities.Animations
{
    public class statement : MonoBehaviour
    {


        public float fadeInDuration = 0.5f;
        public float fallDuration = 0.5f;
        public float fallDistance = 0.5f;
        public float shakeDuration = 0.1f;
        public float shakeStrength = 1f;
        public int shakeVibrato = 10;
        public Camera[] cameras;


        public Material material; // 해당 오브젝트의 매터리얼
        public SpriteRenderer[] childSprites; // 자식 스프라이트
        public float cutOffHeight = -10f; // CutoffHeight의 목표 값
        public float duration = 0.5f; // 애니메이션의 지속시간



        private Renderer itemRenderer;

        private void Start()
        {
            itemRenderer = GetComponent<Renderer>();
            StartCoroutine(AnimateItem());



        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                StartCoroutine(AnimateItemReverse());
            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                StartCoroutine(ActivateBadsector());
            }
        }

        public IEnumerator AnimateItem()
        {
            // 동시에 시작할 애니메이션들을 담은 Sequence를 생성합니다.
            DG.Tweening.Sequence sequence = DOTween.Sequence();

            //
            Vector3 endPosition = transform.position;
            transform.position = new Vector3(transform.position.x, transform.position.y + fallDistance, transform.position.z);
            sequence.Insert(0, transform.DOMove(endPosition, 0.5f).SetEase(Ease.InCubic));

            // Fade in 애니메이션
            sequence.Insert(0, itemRenderer.material.DOFade(0.5f, 0.5f));

            // Sequence를 시작합니다.
            yield return sequence.WaitForCompletion();

            // 카메라 흔들기 애니메이션
            foreach (Camera camera in cameras)
            {
                camera.DOShakePosition(shakeDuration, shakeStrength, shakeVibrato);
            }
        }


        public IEnumerator AnimateItemReverse()
        {
            DG.Tweening.Sequence sequence = DOTween.Sequence();

            Vector3 startPosition = new Vector3(transform.position.x, transform.position.y + 10, transform.position.z);
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            sequence.Insert(0, transform.DOMove(startPosition, 0.5f).SetEase(Ease.InCubic));

            sequence.Insert(0, itemRenderer.material.DOFade(0, 0.5f));

            yield return sequence.WaitForCompletion();
            Destroy(gameObject);
        }

        public IEnumerator ActivateBadsector()
        {
            DG.Tweening.Sequence sequence = DOTween.Sequence();

            MeshRenderer meshRenderer = GetComponent<MeshRenderer>(); // MeshRenderer 컴포넌트를 가져옵니다
            float duration = 0.5f; // 애니메이션 지속시간을 설정합니다

            Vector3 rotationAmount = new Vector3(10, 0, 10);

            // 알파값을 천천히 0으로 변경합니다
            // 애니메이션이 끝나면 자동으로 Destroy(gameObject)를 호출합니다
            sequence.Append(transform.DOPunchRotation(rotationAmount, duration, 10, 1));
            meshRenderer.material.DOFade(0, duration);


            // 자식 스프라이트의 alpha 값을 0으로 변경하는 애니메이션
            foreach (SpriteRenderer sprite in childSprites)
            {
                sequence.Insert(0, sprite.material.DOFade(0, duration)); // alpha 값을 0으로 변화
            }

            yield return sequence.WaitForCompletion();
            Destroy(gameObject);
        }
    }

}
