using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using NUnit.Framework.Internal;

namespace CodingStrategy.Entities.Animations
{
    public class TileAttackAnimation : MonoBehaviour
    {
        
        private Color originalColor;
        public GameObject tileSurface;
        public Material newMaterial;


        private void Awake()
        {
            newMaterial = tileSurface.GetComponent<Renderer>().material;
            originalColor = newMaterial.color; // 원래 색상 저장

        }

        private void Update()
        {
            if(Input.GetKey(KeyCode.K))
            {
                Color test = Color.cyan;
                StartCoroutine(AttackArea(test));
            }
        }

        public IEnumerator AttackArea(Color color)
        {
            
            // 동시에 시작할 애니메이션들을 담은 Sequence를 생성합니다.

            Sequence sequence = DOTween.Sequence();
            
            // 아래로 내려가는 애니메이션과 색상 변경 애니메이션
            Vector3 downPosition = new Vector3(transform.position.x, transform.position.y - 0.1f, transform.position.z);
            
            sequence.Append(transform.DOMove(downPosition, 0.75f).SetEase(Ease.InOutSine)); // 0.75초 동안 아래로 이동
            sequence.Join(newMaterial.DOColor(color, 0.75f)); // 0.75초 동안 색상을 입력값으로 변경
            
            // 원래 위치로 돌아가는 애니메이션과 원래 색상으로 되돌리는 애니메이션
            sequence.Append(transform.DOMove(transform.position, 0.25f).SetEase(Ease.InOutSine)); // 0.25초 동안 원래 위치로 이동
            sequence.Join(newMaterial.DOColor(originalColor, 0.25f)); // 0.25초 동안 색상을 원래 색상으로 되돌림
            
            // Sequence를 시작합니다.
            yield return sequence.WaitForCompletion();

        }
    }
}
