using UnityEngine;
using System.Collections;

public class LilbotController : MonoBehaviour
{
    public Animator animator;
    public float speed;
    public bool Jump;
    public bool attack1;
    public bool attack2;
    public bool Hit;
    public bool Death;


    void Update()
    {
        Walk(speed);
        if (Jump) { JumpAnimation(); }
        if (attack1) { Attack1(); }
        if (attack2) { Attack2(); }
        if (Hit) { HitAnimation(); }
        if (Death) { DeathAnimation(); }
    }

    public void Walk(float speed)
    {
        animator.SetFloat("Speed", speed);
    }

    
    public void JumpAnimation()
    {
        StartCoroutine(JumpAnimationCoroutine());
    }

    IEnumerator JumpAnimationCoroutine()
    {
        animator.SetTrigger("Jump");
        yield return new WaitForSeconds(1);   // 1초 대기
        animator.ResetTrigger("Jump");
    }




    #region attack_Animation
    public void Attack1()
    {
        animator.SetTrigger("Attack1");
    }

    public void Attack2()
    {
        animator.SetTrigger("Attack2");
    }
    #endregion



    #region Death_Animation
    public void DeathAnimation()
    {
        StartCoroutine(DeathAnimationCoroutine());
    }

    IEnumerator DeathAnimationCoroutine()
    {
        animator.SetTrigger("Death");
        yield return new WaitForSeconds(1);   // 1초 대기
        animator.ResetTrigger("Death");
    }
    #endregion



    #region Hit_Animation
    //피격할 때의 애니메이션 작업을 진행합니다.
    public void HitAnimation()
    {
        StartCoroutine(HitAnimationCoroutine());
    }

    IEnumerator HitAnimationCoroutine()
    {
        animator.SetTrigger("Hit");
        yield return new WaitForSeconds(1);   // 1초 대기
        animator.ResetTrigger("Hit");
    }
    #endregion

}