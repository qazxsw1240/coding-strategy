#nullable enable

using CodingStrategy.Entities.Robot;

namespace CodingStrategy.Entities.Runtime.Statement
{
    public class AddArmorStatement : AbstractStatement
    {
        private readonly int _armorPoint;

        public AddArmorStatement(IRobotDelegate robotDelegate, int armorPoint)
            : base(robotDelegate)
        {
            _armorPoint = armorPoint;
        }

        public override StatementPhase Phase
        {
            get { return StatementPhase.Static; }
        }

        public override IStatement Reverse
        {
            get { return new AddEnergyStatement(_robotDelegate, -_armorPoint); }
        }

        public override void Execute(RuntimeExecutorContext context)
        {
            _robotDelegate.ArmorPoint += _armorPoint;
        }
    }
}
