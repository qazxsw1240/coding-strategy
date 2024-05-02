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
        private readonly Queue<IEnumerator> _animations;

        private bool _idle;

        public AnimationCoroutineManager()
        {
            _animations = new Queue<IEnumerator>();
            _idle = true;
        }

        public void Start()
        {
            _animations.Clear();
            _idle = true;
        }

        public void Update()
        {
            if (_idle)
            {
                StartCoroutine(StartParallelCoroutines());
            }
        }

        public void AddAnimation(IEnumerator coroutine)
        {
            _animations.Enqueue(coroutine);
        }

        public void ApplyAnimations()
        {
            if (!_idle)
            {
                return;
            }

            _idle = false;
        }

        private IEnumerator StartParallelCoroutines()
        {
            _idle = false;

            IList<Coroutine> coroutines = new List<Coroutine>();

            while (_animations.TryDequeue(out IEnumerator coroutine))
            {
                coroutines.Add(StartCoroutine(coroutine));
            }

            foreach (Coroutine coroutine in coroutines)
            {
                yield return coroutine;
            }

            _idle = true;
        }
    }
}
