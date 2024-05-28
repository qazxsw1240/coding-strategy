#nullable enable


namespace CodingStrategy.Entities.Runtime.Statement
{
    using Robot;

    public class SuperStatement : AbstractStatement
    {
        private readonly bool _isSuper;

        public SuperStatement(IRobotDelegate robotDelegate, bool isSuper=true)
        : base(robotDelegate)
        {
            _isSuper = isSuper;
        }

        public override void Execute(RuntimeExecutorContext context)
        {
            context.ExecutionQueuePool[_robotDelegate].IsProtected = true;
        }

        public override StatementPhase Phase => StatementPhase.Move;

        public override IStatement Reverse => new SuperStatement(_robotDelegate, !_isSuper);
    }
}
