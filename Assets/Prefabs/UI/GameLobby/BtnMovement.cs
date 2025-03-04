﻿using DG.Tweening;

using UnityEngine;
using UnityEngine.UI;

public class BtnMovement : MonoBehaviour
{
    public static int activeAnimations;

    public ScrollRect scrollView; // 스크롤뷰에 대한 참조를 저장합니다.
    private bool isAnimating; // 애니메이션 상태를 추적하는 플래그 변수입니다.
    private Vector3 originalPos; // 애니메이션 전의 원래 위치를 저장합니다.

    public void AnimateButton()
    {
        Vector3 originalPos = transform.position;
        Vector3 targetPos = originalPos + new Vector3(50, -40, 0);
        if (isAnimating) // 애니메이션이 이미 실행 중이면
        {
            Debug.Log("Animation is already in progress."); // 메시지를 출력하고
            return; // 함수를 종료합니다.
        }
        isAnimating = true;
        Sequence sequence = DOTween.Sequence();
        // 애니메이션 시작 시 activeAnimations 값을 증가
        sequence.AppendCallback(
            () =>
            {
                activeAnimations++;
                scrollView.enabled = false;
            });
        sequence.Append(transform.DOMove(targetPos, 0.5f));
        sequence.Append(transform.DOMove(originalPos, 0.25f));
        // 애니메이션 종료 시 activeAnimations 값을 감소
        sequence.AppendCallback(
            () =>
            {
                activeAnimations--;
                isAnimating = false;
                if (activeAnimations == 0)
                {
                    scrollView.enabled = true;
                }
            });
    }
}
