#nullable enable


namespace CodingStrategy.Entities
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// 오브젝트의 애니메이션을 관리합니다.
    /// </summary>
    public class AnimationCoroutineManager : MonoBehaviour
    {
        private readonly IDictionary<GameObject, Queue<IEnumerator>> _animationQueues =
            new Dictionary<GameObject, Queue<IEnumerator>>();

        private Coroutine? _coroutine;

        public void Start()
        {
            _animationQueues.Clear();
        }

        /// <summary>
        /// 코루틴 애니메이션을 큐에 추가합니다.
        /// </summary>
        /// <param name="target">애니메이션을 적용할 게임 오브젝트입니다.</param>
        /// <param name="coroutine">큐에 추가할 코루틴 애니메이션입니다.</param>
        public void AddAnimation(GameObject target, IEnumerator coroutine)
        {
            if (!_animationQueues.TryGetValue(target, out Queue<IEnumerator> animationQueue))
            {
                animationQueue = new Queue<IEnumerator>();
                _animationQueues[target] = animationQueue;
            }

            animationQueue.Enqueue(coroutine);
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
            Queue<Coroutine> coroutines = new Queue<Coroutine>();
            HashSet<GameObject> emptyTargets = new HashSet<GameObject>();

            while (_animationQueues.Count != 0)
            {
                emptyTargets.Clear();
                foreach ((GameObject target, Queue<IEnumerator> animationQueue) in _animationQueues)
                {
                    if (animationQueue.TryDequeue(out IEnumerator coroutine))
                    {
                        coroutines.Enqueue(StartCoroutine(coroutine));
                    }
                    else
                    {
                        emptyTargets.Add(target);
                    }
                }

                while (coroutines.TryDequeue(out Coroutine coroutine))
                {
                    yield return coroutine;
                }

                foreach (GameObject target in emptyTargets)
                {
                    _animationQueues.Remove(target);
                }
            }


            _coroutine = null;
        }
    }
}
