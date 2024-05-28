#nullable enable


namespace CodingStrategy.Entities.Runtime.Statement
{
    using Robot;

    public class AddEnergyStatement : AbstractStatement
    {
        private readonly int _energyPoint;

        public AddEnergyStatement(IRobotDelegate robotDelegate, int energyPoint)
        : base(robotDelegate)
        {
            _energyPoint = energyPoint;
        }

        public override void Execute(RuntimeExecutorContext context)
        {
            _robotDelegate.EnergyPoint+=_energyPoint;
        }

        public override StatementPhase Phase => StatementPhase.Static;

        public override IStatement Reverse => new AddEnergyStatement(_robotDelegate, _energyPoint);
    }
}
