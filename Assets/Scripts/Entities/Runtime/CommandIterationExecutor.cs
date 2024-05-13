#nullable enable


using System.Collections;

namespace CodingStrategy.Entities.Runtime
{
    using System.Collections.Generic;

    public class CommandIterationExecutor : LifeCycleMonoBehaviourBase, ILifeCycle
    {
        private IList<IExecutionValidator> _validators = null!;
        private IEnumerator<IExecutionValidator> _enumerator = null!;
        private RobotStatementExecutor _executor = null!;

        public RuntimeExecutorContext Context { get; set; }

        public IList<IExecutionValidator> Validators
        {
            get => _validators;
            set
            {
                _validators = value;
                _enumerator = value.GetEnumerator();
            }
        }

        public void Awake()
        {
            LifeCycle = this;
        }

        public void Initialize()
        {
            _enumerator.Reset();
        }

        public bool MoveNext()
        {
            return _enumerator.MoveNext();
        }

        public bool Execute()
        {
            IExecutionValidator validator = _enumerator.Current!;
            _executor = gameObject.AddComponent<RobotStatementExecutor>();
            _executor.Validator = validator;
            _executor.Context = Context;
            return true;
        }

        public void Terminate()
        {
            _enumerator.Dispose();
        }

        protected override IEnumerator OnAfterInitialization()
        {
            yield return null;
        }

        protected override IEnumerator OnBeforeExecution()
        {
            yield return null;
        }

        protected override IEnumerator OnAfterFailExecution()
        {
            yield return null;
        }

        protected override IEnumerator OnAfterExecution()
        {
            yield return null; // _executor.Start() 대기
            yield return _executor.Coroutine;
        }

        protected override IEnumerator OnAfterTermination()
        {
            Destroy(_executor);
            yield return null;
        }
    }
}
