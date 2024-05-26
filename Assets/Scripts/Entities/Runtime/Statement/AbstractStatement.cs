#nullable enable


namespace CodingStrategy.Entities.Runtime.Statement
{
    using Robot;
    public abstract class AbstractStatement : IStatement
    {
        protected readonly IRobotDelegate _robotDelegate;
        protected readonly int _energy;
        protected AbstractStatement(IRobotDelegate robotDelegate, int energy)
        {
            _robotDelegate = robotDelegate;
            _energy = energy;
        }
        public virtual void Execute(RuntimeExecutorContext context)
        {
            if(_robotDelegate.EnergyPoint<_energy)
            {
                throw new ExecutionException();
            }
        }
        public abstract StatementPhase Phase { get; }
        public abstract IStatement Reverse { get; }
    }
}