using System.Collections;

using CodingStrategy.Sound;

using DG.Tweening;

using UnityEngine;

namespace CodingStrategy.Animation
{
    public class BadSectorAnimation : MonoBehaviour
    {
        private static readonly int Surface = Shader.PropertyToID("_Surface");

        public float fadeInDuration = 0.5f;
        public float fallDuration = 0.5f;
        public float fallDistance = 5f;
        public float shakeDuration = 0.3f;
        public float shakeStrength = 0.2f;
        public int shakeVibrato = 1;

        [SerializeField]
        private Camera _camera;

        public Shader shader;
        public Material newMaterial;

        [SerializeField]
        private Renderer itemRenderer;

        [SerializeField]
        private SpriteRenderer[] childSprites;

        public Camera Camera
        {
            set => _camera = value;
        }

        private void Awake()
        {
            newMaterial = gameObject.GetComponent<Renderer>().material;
            newMaterial.SetFloat(Surface, 1f);
            itemRenderer = gameObject.GetComponent<Renderer>();
            itemRenderer.material = newMaterial;
        }

        public IEnumerator AnimateItem()
        {
            Sequence sequence = DOTween.Sequence();
            Vector3 endPosition = transform.position;
            transform.position = new Vector3(
                transform.position.x,
                transform.position.y + fallDistance,
                transform.position.z);
            sequence.Insert(0, transform.DOMove(endPosition, fallDuration).SetEase(Ease.InCubic));
            sequence.Insert(0, itemRenderer.material.DOFade(0.5f, fadeInDuration));
            yield return sequence.WaitForCompletion();
            AudioClip effectClip = Resources.Load<AudioClip>("Sound/GameScene_BadSctor_Sound");
            SoundManager.Instance.Play(effectClip);
            yield return _camera.DOShakePosition(shakeDuration, shakeStrength, shakeVibrato).WaitForCompletion();
            Debug.Log("Bad sector install animation ended.");
        }

        public IEnumerator AnimateItemReverse()
        {
            Sequence sequence = DOTween.Sequence();

            Vector3 startPosition = new Vector3(transform.position.x, transform.position.y + 10, transform.position.z);
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            sequence.Insert(0, transform.DOMove(startPosition, fallDuration).SetEase(Ease.OutCubic));
            sequence.Insert(0, itemRenderer.material.DOFade(0, fadeInDuration));
            foreach (SpriteRenderer sprite in childSprites)
            {
                sequence.Insert(0, sprite.material.DOFade(0, 0.5f)); // alpha 값을 0으로 변화
            }
            yield return sequence.WaitForCompletion();
            Destroy(gameObject);
        }

        public IEnumerator ActivateBadSector()
        {
            const float duration = 0.5f; // 애니메이션 지속시간을 설정합니다
            Vector3 punchAmount = new Vector3(10, 0, 10); // X축과 Z축을 기준으로 10도 흔들립니다
            Sequence sequence = DOTween.Sequence();
            sequence.Append(transform.DOPunchRotation(punchAmount, duration));
            GetComponent<MeshRenderer>().material.DOFade(0, duration).SetEase(Ease.OutCubic);
            AudioClip effectClip = Resources.Load<AudioClip>("Sound/GameScene_BadscetorActivate_Sound");
            SoundManager.Instance.Play(effectClip);
            Debug.Log("Bad sector activate sound is coming out!");
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
            newMaterial.color = color;
        }
    }
}
