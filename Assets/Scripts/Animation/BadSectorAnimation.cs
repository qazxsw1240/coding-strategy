using DG.Tweening;
using JetBrains.Annotations;
using System;
using System.Collections;
using UnityEngine;

namespace CodingStrategy.Entities.Animations
{
    public class BadSectorAnimation : MonoBehaviour
    {
        public float fadeInDuration = 0.5f;
        public float fallDuration = 0.5f;
        public float fallDistance = 5f;
        public float shakeDuration = 0.3f;
        public float shakeStrength = 0.2f;
        public int shakeVibrato = 1;
        public Camera camera;

        public Shader shader;
        public Material newMaterial;

        private Renderer _itemRenderer;
        public SpriteRenderer[] childSprites; // 자식 스프라이트
        private static readonly int Surface = Shader.PropertyToID("_Surface");

        private void Awake()
        {
            // shader = Shader.Find("Universal Render Pipeline/Lit");
            // newMaterial = new Material(shader);
            newMaterial = gameObject.GetComponent<Renderer>().material;
            newMaterial.SetFloat(Surface, 1f);
            _itemRenderer = gameObject.GetComponent<Renderer>();
            _itemRenderer.material = newMaterial;
        }


        private void Start()
        {
            // _itemRenderer = gameObject.GetComponent<Renderer>();
            // _itemRenderer.material = newMaterial;
            //StartCoroutine(AnimateItem());
        }

        public IEnumerator AnimateItem()
        {
            // 동시에 시작할 애니메이션들을 담은 Sequence를 생성합니다.
            Sequence sequence = DOTween.Sequence();

            //
            Vector3 endPosition = transform.position;
            transform.position = new Vector3(transform.position.x, transform.position.y + fallDistance,
                transform.position.z);
            sequence.Insert(0, transform.DOMove(endPosition, fallDuration).SetEase(Ease.InCubic));

            // Fade in 애니메이션
            sequence.Insert(0, _itemRenderer.material.DOFade(0.5f, fadeInDuration));

            // Sequence를 시작합니다.
            yield return sequence.WaitForCompletion();

            // 카메라 흔들기 애니메이션

            yield return camera.DOShakePosition(shakeDuration, shakeStrength, shakeVibrato).WaitForCompletion();

            Debug.Log("Bad sector install animation ended.");
        }


        public IEnumerator AnimateItemReverse()
        {
            Sequence sequence = DOTween.Sequence();

            Vector3 startPosition = new Vector3(transform.position.x, transform.position.y + 10, transform.position.z);
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            sequence.Insert(0, transform.DOMove(startPosition, fallDuration).SetEase(Ease.OutCubic));


            sequence.Insert(0, _itemRenderer.material.DOFade(0, fadeInDuration));
            foreach (SpriteRenderer sprite in childSprites)
            {
                sequence.Insert(0, sprite.material.DOFade(0, 0.5f)); // alpha 값을 0으로 변화
            }

            yield return sequence.WaitForCompletion();
            Destroy(gameObject);
        }

        public IEnumerator ActivateBadsector()
        {
            float duration = 0.5f; // 애니메이션 지속시간을 설정합니다
            Vector3 punchAmount = new Vector3(10, 0, 10); // X축과 Z축을 기준으로 10도 흔들립니다
            DG.Tweening.Sequence sequence = DOTween.Sequence();

            // X축과 Z축을 기준으로 오브젝트를 살짝 흔드는 애니메이션을 추가합니다
            sequence.Append(transform.DOPunchRotation(punchAmount, duration, 10, 1));

            // 알파값을 천천히 0으로 변경합니다
            // 애니메이션이 끝나면 자동으로 Destroy(gameObject)를 호출합니다
            GetComponent<MeshRenderer>().material.DOFade(0, duration).SetEase(Ease.OutCubic);

            foreach (SpriteRenderer sprite in childSprites)
            {
                sequence.Insert(0, sprite.material.DOFade(0, duration)); // alpha 값을 0으로 변화
            }

            yield return sequence.WaitForCompletion();
            Destroy(gameObject);
        }

        public void ChangeBadSectorColor(Color color)
        {
            gameObject.GetComponent<Renderer>().material = newMaterial;
            //Color Red = new Vector4(224, 0, 4, 204) / 255;
            //Color Green = new Vector4(83, 219, 57, 255) / 255;
            //Color Blue = new Vector4(78, 149, 217, 255) / 255;
            //Color Yellow = new Vector4(245, 184, 0, 255) / 255;
            //
            //if (ColorString == "R") 
            //{
            //    newMaterial.color = Red;
            //}
            //else if (ColorString == "Y")
            //{
            //    newMaterial.color = Yellow;
            //}
            //else if (ColorString == "G")
            //{
            //    newMaterial.color = Green;
            //}
            //else if (ColorString == "B")
            //{
            //    newMaterial.color = Blue;
            //}
            newMaterial.color = color;
        }
    }
}
