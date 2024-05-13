#nullable enable


namespace CodingStrategy.Entities
{
    using System.Collections;
    using UnityEngine;

    public abstract class LifeCycleMonoBehaviourBase : MonoBehaviour
    {
        public static IEnumerator AwaitLifeCycleCoroutine<TLifeCycle>(TLifeCycle lifeCycle,
            bool removeIfComplete = true)
            where TLifeCycle : LifeCycleMonoBehaviourBase
        {
            if (!lifeCycle._starts)
            {
                yield return null; // Start() 대기
            }

            yield return new WaitUntil(() => lifeCycle.Coroutine == null);

            if (removeIfComplete)
            {
                Destroy(lifeCycle);
            }

            yield return null;
        }

        private bool _starts = false;

        public void Start()
        {
            Coroutine = StartCoroutine(StartLifeCycleCoroutine());
            _starts = true;
        }

        public Coroutine? Coroutine { get; private set; }

        public ILifeCycle LifeCycle { get; set; } = null!;

        protected abstract IEnumerator OnAfterInitialization();

        protected abstract IEnumerator OnBeforeExecution();

        protected abstract IEnumerator OnAfterFailExecution();

        protected abstract IEnumerator OnAfterExecution();

        protected abstract IEnumerator OnAfterTermination();

        private IEnumerator StartLifeCycleCoroutine()
        {
            LifeCycle.Initialize();

            yield return StartCoroutine(OnAfterInitialization());

            while (LifeCycle.MoveNext())
            {
                yield return StartCoroutine(OnBeforeExecution());

                bool result = LifeCycle.Execute();

                if (!result)
                {
                    yield return StartCoroutine(OnAfterFailExecution());
                    break;
                }

                yield return StartCoroutine(OnAfterExecution());
            }

            LifeCycle.Terminate();

            yield return StartCoroutine(OnAfterTermination());

            Coroutine = null;
        }
    }
}
