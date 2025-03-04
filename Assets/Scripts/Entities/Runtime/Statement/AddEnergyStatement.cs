#nullable enable

using CodingStrategy.Entities.Robot;

namespace CodingStrategy.Entities.Runtime.Statement
{
    public class AddEnergyStatement : AbstractStatement
    {
        private readonly int _energyPoint;

        public AddEnergyStatement(IRobotDelegate robotDelegate, int energyPoint)
            : base(robotDelegate)
        {
            _energyPoint = energyPoint;
        }

        public override StatementPhase Phase
        {
            get { return StatementPhase.Static; }
        }

        public override IStatement Reverse
        {
            get { return new AddEnergyStatement(_robotDelegate, -_energyPoint); }
        }

        public override void Execute(RuntimeExecutorContext context)
        {
            _robotDelegate.EnergyPoint += _energyPoint;
        }
    }
}
