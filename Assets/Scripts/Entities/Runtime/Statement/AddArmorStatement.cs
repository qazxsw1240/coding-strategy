#nullable enable


namespace CodingStrategy.Entities.Runtime.Statement
{
    using Robot;

    public class AddArmorStatement : AbstractStatement
    {
        private readonly int _armorPoint;

        public AddArmorStatement(IRobotDelegate robotDelegate, int armorPoint)
        : base(robotDelegate)
        {
            _armorPoint = armorPoint;
        }

        public override void Execute(RuntimeExecutorContext context)
        {
            _robotDelegate.ArmorPoint+=_armorPoint;
        }

        public override StatementPhase Phase => StatementPhase.Static;

        public override IStatement Reverse => new AddEnergyStatement(_robotDelegate, -_armorPoint);
    }
}
