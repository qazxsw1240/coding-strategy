#nullable enable


namespace CodingStrategy.Entities
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// 오브젝트의 애니메이션을 관리합니다.
    /// </summary>
    public class AnimationCoroutineManager : MonoBehaviour
    {
        private readonly Queue<IEnumerator> _animations = new Queue<IEnumerator>();

        private Coroutine? _coroutine;

        public void Start()
        {
            _animations.Clear();
        }

        /// <summary>
        /// 코루틴 애니메이션을 큐에 추가합니다.
        /// </summary>
        /// <param name="coroutine">큐에 추가할 코루틴 애니메이션입니다.</param>
        public void AddAnimation(IEnumerator coroutine)
        {
            _animations.Enqueue(coroutine);
        }

        /// <summary>
        /// 큐에 저장된 코루틴 애니메이션을 실행합니다.
        /// </summary>
        /// <returns>저장된 애니메이션을 완료하기를 기다리는 코루틴을 반환합니다.</returns>
        public Coroutine ApplyAnimations()
        {
            if (_coroutine != null)
            {
                return _coroutine;
            }

            _coroutine = StartCoroutine(StartParallelCoroutines());

            return _coroutine;
        }

        private IEnumerator StartParallelCoroutines()
        {
            IList<Coroutine> coroutines = new List<Coroutine>();

            while (_animations.TryDequeue(out IEnumerator coroutine))
            {
                coroutines.Add(StartCoroutine(coroutine));
            }

            foreach (Coroutine coroutine in coroutines)
            {
                yield return coroutine;
            }

            _coroutine = null;
        }
    }
}
