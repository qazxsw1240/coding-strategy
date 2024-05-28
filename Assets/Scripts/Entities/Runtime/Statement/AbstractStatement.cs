#nullable enable


namespace CodingStrategy.Entities.Runtime.Statement
{
    using Robot;
    public abstract class AbstractStatement : IStatement
    {
        protected readonly IRobotDelegate _robotDelegate;
        protected AbstractStatement(IRobotDelegate robotDelegate)
        {
            _robotDelegate = robotDelegate;
        }
        public abstract void Execute(RuntimeExecutorContext context);
        public abstract StatementPhase Phase { get; }
        public abstract IStatement Reverse { get; }
    }
}