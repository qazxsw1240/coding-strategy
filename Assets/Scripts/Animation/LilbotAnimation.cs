using System.Collections;

using CodingStrategy.Entities.Animations;
using CodingStrategy.Sound;

using DG.Tweening;

using UnityEngine;
using UnityEngine.UI;

namespace CodingStrategy.Animation
{
    public class LilbotAnimation : MonoBehaviour
    {
        private static readonly int Speed = Animator.StringToHash("Speed");
        private static readonly int Jump = Animator.StringToHash("Jump");
        private static readonly int Attack1 = Animator.StringToHash("Attack1");
        private static readonly int Attack2 = Animator.StringToHash("Attack2");
        private static readonly int Death = Animator.StringToHash("Death");
        private static readonly int Hit = Animator.StringToHash("Hit");

        public Animator animator;

        public bool jump;
        public bool attack1;
        public bool attack2;
        public bool hit;
        public bool death;

        public float duration = 1f;
        public Camera playerCamera;

        [SerializeField]
        private LilbotStatement lilbotStatement;

        [SerializeField]
        private Image[] statements;

        public void Start()
        {
            lilbotStatement = gameObject.GetComponent<LilbotStatement>();
            statements = lilbotStatement.statements;
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                StartCoroutine(StatementActive("Trojan"));
            }
        }

        public IEnumerator Walk(float speed, int x, int z)
        {
            //speed의 값이 1에 가까우면 가까울수록 달리는 애니메이션이 틀어질거에요.
            //1이면 나루토 달리기, 0.5면 뚜방뚜방 걷기, 0이면 가만히 서있기를 실행합니다.
            animator.SetFloat(Speed, speed);
            Vector3 targetPosition = new Vector3(x, transform.position.y, z);
            ISoundManager soundManager = SoundManager.Instance;
            AudioClip effectClip = Resources.Load<AudioClip>("Sound/GameScene_CharacterWalk_Sound");
            soundManager.Play(effectClip);
            Debug.Log("Character walk sound is coming out!");
            transform.DOMove(targetPosition, duration).SetEase(Ease.Linear);
            yield return new WaitForSeconds(duration);
            animator.SetFloat(Speed, 0);
        }

        public IEnumerator SpawnAnimationCoroutine()
        {
            Vector3 returnPosition = gameObject.transform.position;
            Vector3 endPosition = new Vector3(transform.position.x, transform.position.y + 1.3f, transform.position.z);
            gameObject.transform.position =
                new Vector3(transform.position.x, transform.position.y + 10.0f, transform.position.z);
            Sequence sequence = DOTween.Sequence();
            sequence.AppendCallback(() => animator.SetTrigger(Jump));
            sequence.Insert(0, transform.DOMove(endPosition, 0.8f).SetEase(Ease.OutCubic));
            sequence.AppendCallback(() => animator.ResetTrigger(Jump));
            sequence.Append(transform.DOMove(returnPosition, 0.2f));
            yield return sequence.WaitForCompletion();
        }

#region Death_Animation

        public IEnumerator DeathAnimationCoroutine()
        {
            animator.SetTrigger(Death);
            // 죽고 나오는 사운드 GameScene_RobotDeath_Sound
            ISoundManager soundManager = SoundManager.Instance;
            AudioClip effectClip = Resources.Load<AudioClip>("Sound/GameScene_RobotDeath_Sound");
            soundManager.Play(effectClip, SoundType.Effect, 1.0f, 0.5f);
            playerCamera.DOShakePosition(1);
            yield return new WaitForSeconds(1); // 1초 대기
            animator.ResetTrigger(Death);
        }

#endregion

#region Hit_Animation

        //피격할 때의 애니메이션 작업을 진행합니다.
        public IEnumerator HitAnimationCoroutine()
        {
            animator.SetTrigger(Hit);
            ISoundManager soundManager = SoundManager.Instance;
            AudioClip effectClip = Resources.Load<AudioClip>("Sound/GameScene_GotAttacked_Sound");
            soundManager.Play(effectClip, SoundType.Effect, 1.0f, 0.5f);
            playerCamera.DOShakePosition(1, 1);
            yield return new WaitForSeconds(1); // 1초 대기
            animator.ResetTrigger(Hit);
        }

#endregion

#region define Statement

        private IEnumerator StatementActive(string statement)
        {
            animator.SetTrigger(Hit);
            switch (statement)
            {
                case "Trojan":
                    statements[0].gameObject.SetActive(true);
                    break;
                case "Malware":
                    statements[1].gameObject.SetActive(true);
                    break;
            }
            yield return new WaitForSeconds(0.8f); // 0.8초 대기
            for (int i = 0; i < statement.Length; i++)
            {
                statements[i].gameObject.SetActive(false);
            }
            animator.ResetTrigger(Hit);
        }

#endregion

#region attack_Animation

        //로봇 기준 오른손 어퍼컷
        public IEnumerator AttackRightAnimationCoroutine()
        {
            animator.SetTrigger(Attack1);
            // 오른손 어퍼컷 사운드. 왼손과 동일
            ISoundManager soundManager = SoundManager.Instance;
            AudioClip effectClip = Resources.Load<AudioClip>("Sound/GameScene_RobotAttacked_Sound");
            soundManager.Play(effectClip, SoundType.Effect, 1.0f, 0.5f);
            yield return new WaitForSeconds(1); // 1초 대기
            animator.ResetTrigger(Attack1);
        }

        //로봇 기준 왼손 어퍼컷
        public IEnumerator AttackLeftAnimationCoroutine()
        {
            animator.SetTrigger(Attack2);
            // 왼손 어퍼컷 사운드. 오른손과 동일
            ISoundManager soundManager = SoundManager.Instance;
            AudioClip effectClip = Resources.Load<AudioClip>("Sound/GameScene_RobotAttacked_Sound");
            soundManager.Play(effectClip, SoundType.Effect, 1.0f, 0.5f);
            yield return new WaitForSeconds(1); // 1초 대기
            animator.ResetTrigger(Attack2);
        }

#endregion
    }
}
