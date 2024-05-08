#nullable enable


namespace CodingStrategy.Entities
{
    using System.Collections;
    using UnityEngine;

    public abstract class LifeCycleMonoBehaviourBase : MonoBehaviour
    {
        private Coroutine? _coroutine;

        public void Start()
        {
            _coroutine = StartCoroutine(StartLifeCycleCoroutine());
        }

        public bool HasCoroutine => _coroutine != null;

        public ILifeCycle LifeCycle { get; set; } = null!;

        protected abstract IEnumerator OnAfterInitialization();

        protected abstract IEnumerator OnBeforeExecution();

        protected abstract IEnumerator OnAfterFailExecution();

        protected abstract IEnumerator OnAfterExecution();

        protected abstract IEnumerator OnAfterTermination();

        private IEnumerator StartLifeCycleCoroutine()
        {
            Debug.Log($"Start LifeCycle: {LifeCycle.GetType().Name}");

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

            Debug.Log($"Ends LifeCycle: {LifeCycle.GetType().Name}");

            _coroutine = null;
        }
    }
}
