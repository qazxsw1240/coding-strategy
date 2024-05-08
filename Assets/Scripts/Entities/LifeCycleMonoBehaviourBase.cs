#nullable enable


namespace CodingStrategy.Entities
{
    using System.Collections;
    using UnityEngine;

    public abstract class LifeCycleMonoBehaviourBase : MonoBehaviour
    {
        private Coroutine? _coroutine;

        protected LifeCycleMonoBehaviourBase(ILifeCycle lifeCycle)
        {
            LifeCycle = lifeCycle;
        }

        public void Start()
        {
            _coroutine = StartCoroutine(StartLifeCycleCoroutine());
        }

        public bool HasCoroutine => _coroutine != null;

        public ILifeCycle LifeCycle { get; }

        protected abstract IEnumerator OnAfterInitialization();

        protected abstract IEnumerator OnBeforeExecution();

        protected abstract IEnumerator OnAfterFailExecution();

        protected abstract IEnumerator OnAfterExecution();

        protected abstract IEnumerator OnAfterTermination();

        private IEnumerator StartLifeCycleCoroutine()
        {
            LifeCycle.Initialize();

            yield return OnAfterInitialization();

            while (LifeCycle.MoveNext())
            {
                yield return OnBeforeExecution();

                if (!LifeCycle.Execute())
                {
                    yield return OnAfterFailExecution();
                    break;
                }

                yield return OnAfterExecution();
            }

            LifeCycle.Terminate();

            yield return OnAfterTermination();

            _coroutine = null;
        }
    }
}
