#nullable enable


namespace CodingStrategy.Entities.Runtime.Statement
{
    using CodingStrategy.Entities.Player;
    using CodingStrategy.Entities.Runtime.Abnormality;
    using Robot;

    public class SuperStatement : IStatement
    {
        private readonly IRobotDelegate _robotDelegate;
        private readonly bool _isSuper;

        public SuperStatement(IRobotDelegate robotDelegate, bool isSuper=true)
        {
            _robotDelegate = robotDelegate;
            _isSuper = isSuper;
        }

        public void Execute(RuntimeExecutorContext context)
        {
            context.ExecutionQueuePool[_robotDelegate].IsProtected = true;
        }

        public StatementPhase Phase => StatementPhase.Static;

        public IStatement Reverse => new SuperStatement(_robotDelegate, !_isSuper);
    }
}
