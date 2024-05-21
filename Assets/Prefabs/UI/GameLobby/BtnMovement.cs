using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Unity.VisualScripting;

public class BtnMovement : MonoBehaviour
{
    public ScrollRect scrollView; // 스크롤뷰에 대한 참조를 저장합니다.
    private Vector3 originalPos; // 애니메이션 전의 원래 위치를 저장합니다.
    private bool isAnimating = false; // 애니메이션 상태를 추적하는 플래그 변수입니다.

    public void AnimateButton()
    {
        if (isAnimating) // 애니메이션이 이미 실행 중이면
        {
            Debug.Log("Animation is already in progress."); // 메시지를 출력하고
            return; // 함수를 종료합니다.
        }

        isAnimating = true; // 애니메이션 상태를 '실행 중'으로 설정합니다.

        Vector3 originalPos = this.transform.position; // 오브젝트의 현재 위치를 저장합니다.
        Vector3 targetPos = originalPos + new Vector3(50, -40, 0); // 목표 위치를 계산합니다.

        DG.Tweening.Sequence sequence = DG.Tweening.DOTween.Sequence(); // 새로운 DoTween 시퀀스를 생성합니다.
        sequence.AppendCallback(() => scrollView.enabled = false); // 애니메이션 시작 시 스크롤뷰를 비활성화합니다.
        sequence.Append(this.transform.DOMove(targetPos, 0.5f)); // 오브젝트를 목표 위치로 0.5초 동안 이동시킵니다.
        sequence.Append(this.transform.DOMove(originalPos, 0.5f)); // 오브젝트를 원래 위치로 0.5초 동안 이동시킵니다.
        sequence.AppendCallback(() =>
        {
            scrollView.enabled = true; // 애니메이션이 끝나면 스크롤뷰를 다시 활성화합니다.
            isAnimating = false; // 애니메이션 상태를 '종료'로 설정합니다.
        });
    }
}
