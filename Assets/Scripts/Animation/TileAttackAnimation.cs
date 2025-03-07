using System.Collections;

using DG.Tweening;

using UnityEngine;

namespace CodingStrategy.Animation
{
    public class TileAttackAnimation : MonoBehaviour
    {
        [SerializeField]
        private Color originalColor;

        [SerializeField]
        private GameObject tileSurface;

        [SerializeField]
        private Material newMaterial;

        private void Awake()
        {
            newMaterial = tileSurface.GetComponent<Renderer>().material;
            originalColor = newMaterial.color; // 원래 색상 저장
        }

        private void Update()
        {
            if (!Input.GetKey(KeyCode.K))
            {
                return;
            }
            Color test = Color.cyan;
            StartCoroutine(AttackArea(test));
        }

        public IEnumerator AttackArea(Color color)
        {
            Sequence sequence = DOTween.Sequence();
            Vector3 downPosition = new Vector3(transform.position.x, transform.position.y - 0.1f, transform.position.z);
            sequence.Append(transform.DOMove(downPosition, 0.75f).SetEase(Ease.InOutSine));
            sequence.Join(newMaterial.DOColor(color, 0.75f));
            sequence.Append(transform.DOMove(transform.position, 0.25f).SetEase(Ease.InOutSine));
            sequence.Join(newMaterial.DOColor(originalColor, 0.25f));
            yield return sequence.WaitForCompletion();
        }
    }
}
