using UnityEngine;
using System.Collections;
using DG.Tweening;

public class LilbotController : MonoBehaviour
{
    public Animator animator;
    
    public bool Jump;
    public bool attack1;
    public bool attack2;
    public bool Hit;
    public bool Death;

    public float duration = 1f;
    public Camera playercamera;

    public IEnumerator Walk(float speed, int x, int z)
    {
        //speed의 값이 1에 가까우면 가까울수록 달리는 애니메이션이 틀어질거에요.
        //1이면 나루토 달리기, 0.5면 뚜방뚜방 걷기, 0이면 가만히 서있기를 실행합니다.
        animator.SetFloat("Speed", speed);


        // 현재 객체를 (x, y, z) 위치로 speed 속도로 이동합니다. 그러니까,
        Vector3 targetPosition = new Vector3(x, transform.position.y, z);


        // DoTween의 DoMove 함수를 사용하여 이동합니다.
        transform.DOMove(targetPosition, duration).SetEase(Ease.Linear);


        // 이동 완료까지 기다립니다.
        yield return new WaitForSeconds(duration);


        // 이동이 완료되면 애니메이션 Speed를 0으로 설정하여 Idle 상태로 돌아갑니다.
        animator.SetFloat("Speed", 0);
    }
    
    public IEnumerator JumpAnimationCoroutine()
    {
        animator.SetTrigger("Jump");
        yield return new WaitForSeconds(1);   // 1초 대기
        animator.ResetTrigger("Jump");
    }




    #region attack_Animation

    //로봇 기준 오른손 어퍼컷
    public IEnumerator AttackRightAnimationCoroutine()
    {
        animator.SetTrigger("Attack1");
        yield return new WaitForSeconds(1);   // 1초 대기
        animator.ResetTrigger("Attack1");
    }

    //로봇 기준 왼손 어퍼컷
    public IEnumerator AttackLeftAnimationCoroutine()
    {
        animator.SetTrigger("Attack2");
        yield return new WaitForSeconds(1);   // 1초 대기
        animator.ResetTrigger("Attack2");
    }
    #endregion



    #region Death_Animation
    public IEnumerator DeathAnimationCoroutine()
    {
        animator.SetTrigger("Death");

        playercamera.DOShakePosition(1, 5);
        yield return new WaitForSeconds(1);   // 1초 대기
        animator.ResetTrigger("Death");
    }
    #endregion



    #region Hit_Animation
    //피격할 때의 애니메이션 작업을 진행합니다.
    public IEnumerator HitAnimationCoroutine()
    {
        animator.SetTrigger("Hit");
        playercamera.DOShakePosition(1, 3);
        yield return new WaitForSeconds(1);   // 1초 대기
        animator.ResetTrigger("Hit");
    }
    #endregion

}